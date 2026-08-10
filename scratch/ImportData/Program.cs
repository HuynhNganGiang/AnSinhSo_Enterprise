using System;
using Microsoft.Data.SqlClient;
using System.IO;
using BCrypt.Net;

namespace ImportData
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=localhost;Database=AnSinhSoDb;Trusted_Connection=True;TrustServerCertificate=True;";
            string rolesFile = @"D:\AnSinhSo_Enterprise\Data\Stg_Roles.csv";
            string usersFile = @"D:\AnSinhSo_Enterprise\Data\Stg_Users.csv";

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var delCmd = new SqlCommand("DELETE FROM Users; DELETE FROM Roles;", conn);
                delCmd.ExecuteNonQuery();
                Console.WriteLine("Cleared existing data.");

                // 1. Import Roles
                var roleLines = File.ReadAllLines(rolesFile);
                // Skip header
                for (int i = 1; i < roleLines.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(roleLines[i])) continue;
                    var cols = roleLines[i].Split(',');
                    if (cols.Length >= 3)
                    {
                        string maRole = cols[0].Trim();
                        string tenRole = cols[1].Trim();
                        string ghiChu = cols[2].Trim();

                        var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Roles WHERE MaRole = @MaRole", conn);
                        checkCmd.Parameters.AddWithValue("@MaRole", maRole);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count == 0)
                        {
                            var cmd = new SqlCommand("INSERT INTO Roles (MaRole, TenRole, GhiChu) VALUES (@MaRole, @TenRole, @GhiChu)", conn);
                            cmd.Parameters.AddWithValue("@MaRole", maRole);
                            cmd.Parameters.AddWithValue("@TenRole", tenRole);
                            cmd.Parameters.AddWithValue("@GhiChu", ghiChu);
                            cmd.ExecuteNonQuery();
                            Console.WriteLine($"Inserted Role: {maRole}");
                        }
                    }
                }

                // 2. Import Users
                var userLines = File.ReadAllLines(usersFile);
                for (int i = 1; i < userLines.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(userLines[i])) continue;
                    var cols = userLines[i].Split(',');
                    if (cols.Length >= 6)
                    {
                        string username = cols[0].Trim();
                        string rawPassword = cols[1].Trim(); // this is "HASH_ADMIN_123" etc in CSV
                        string hoTen = cols[2].Trim();
                        string email = cols[3].Trim();
                        string maRole = cols[4].Trim();
                        string trangThai = cols[5].Trim();
                        string ghiChu = cols.Length > 6 ? cols[6].Trim() : "";

                        // Generate fake email if empty
                        if (string.IsNullOrEmpty(email))
                        {
                            email = $"{username}@ansinhso.gov.vn";
                        }

                        // Generate actual hash using EnhancedHashPassword to match PasswordHasher
                        string passwordText = rawPassword; 
                        string hash = BCrypt.Net.BCrypt.EnhancedHashPassword(passwordText, 12);

                        string securityStamp = Guid.NewGuid().ToString("N");

                        var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username", conn);
                        checkCmd.Parameters.AddWithValue("@Username", username);
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            var cmd = new SqlCommand(@"
                                INSERT INTO Users (Id, Username, Email, PasswordHash, SecurityStamp, HoTen, MaRole, TrangThai, GhiChu, CreatedAt)
                                VALUES (@Id, @Username, @Email, @PasswordHash, @SecurityStamp, @HoTen, @MaRole, @TrangThai, @GhiChu, @CreatedAt)
                            ", conn);
                            cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                            cmd.Parameters.AddWithValue("@Username", username);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.Parameters.AddWithValue("@PasswordHash", hash);
                            cmd.Parameters.AddWithValue("@SecurityStamp", securityStamp);
                            cmd.Parameters.AddWithValue("@HoTen", hoTen);
                            cmd.Parameters.AddWithValue("@MaRole", maRole);
                            cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                            cmd.Parameters.AddWithValue("@GhiChu", ghiChu);
                            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);

                            cmd.ExecuteNonQuery();
                            Console.WriteLine($"Inserted User: {username}");
                        }
                    }
                }

                Console.WriteLine("Import complete!");
            }
        }
    }
}
