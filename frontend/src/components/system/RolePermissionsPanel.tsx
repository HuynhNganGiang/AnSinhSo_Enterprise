import {
  useMemo,
  useState,
} from 'react'

import './RolePermissionsPanel.css'


type DataSource =
  | 'REAL SNAPSHOT'
  | 'DEMO LOCAL'


type RoleItem = {
  id: string
  name: string
  description: string
  isSystemRole: boolean
  assignedUsers: number
  permissionIds: string[]
  source: DataSource
}


type PermissionItem = {
  id: string
  code: string
  name: string
  description: string
  groupId: string
  groupCode: string
  groupName: string
}


type PermissionGroupItem = {
  id: string
  code: string
  name: string
  description: string
}


type RoleForm = {
  name: string
  description: string
  permissionIds: string[]
}


type ActiveTab =
  | 'roles'
  | 'permissions'
  | 'matrix'


const STORAGE_KEY =
  'ansinhso.demo.system-roles.v1'


const PERMISSION_PAGE_SIZE =
  10


const REAL_ROLES:
  RoleItem[] =
[{"id":"00000000-0000-0000-0000-000000000001","name":"Admin","description":"Qu?n tr? viên h? th?ng","isSystemRole":true,"assignedUsers":1,"permissionIds":["CAA8F0DF-B090-431E-8CB1-01ECB545719A","1284C282-8DAB-4B6D-B4D7-09B13BF8010B","ECE05B02-9709-4AE9-9E50-0E08A4539891","FAF47852-71C8-417D-9A9E-1409CF5E3597","9E0C4E21-061E-4715-8CDE-18507D7A8BF5","75E6013E-C264-4923-A0F1-19CDA4619BF2","A7C070B3-C3F7-4297-8B27-1FF31DAB4414","1DCF9282-8925-4500-95FF-21B93C696972","16434DFC-339C-4F08-B01C-32049E4F5F64","D6531B08-52FF-4367-8161-4545BEE8C383","74497B86-0A4D-4C4E-BB2A-485A51ACAF27","C5D7A3A2-6461-40D1-917D-512E0EE1B918","A67C0DA5-A65A-44C2-AFA0-5A1CDB145FC7","19532748-2935-4866-9230-5B8BEECD692F","D7567A3B-8FD5-4A86-AFB7-60FF9C6815FD","73DF4888-5248-4511-ADBD-618B4DE0455A","16C28C87-2693-43ED-925E-624CA429B94A","A394872C-2266-49B4-A131-688EE3AF1826","8B1E939B-739F-4CBD-87CA-74D068347EE8","975C9923-1FCA-4BBA-86CB-7B014E38F078","FD5F052F-B2FB-4C64-9966-812510A9346E","C5B650C7-18C3-4A82-8ACD-8138400221AB","A5D2BFBF-02BD-4F56-AEA1-8805EAA722CA","A71961F8-C704-4582-B756-9510B95B4DCA","72D3E121-D262-4BC5-AF7F-956D12C27A74","B7C5D1DC-AF2B-44EA-8B5C-98E7FBE53D74","7D7ADAC4-DDB0-4F73-8ED6-A3D597EA37AF","E5862E66-939A-4819-8801-A61000A54783","80420FA3-D9DA-4B07-B91C-AAE5BEED3308","8496A685-755C-4036-83BD-B0F451394464","2CA51CDF-D31C-428E-907E-BB25E471B676","B5479EEA-F849-47FF-A3FF-BC1F9F36CFDD","B4588249-8D59-4515-B651-BDD965E9FBC1","4486116B-3C6D-438B-979B-BE7C3FF306F2","FEBEDAFB-20E5-405F-96D7-C60196B6EE9E","603CC6E5-0AEF-4DFC-B7A1-C8D35CD36205","6002439C-1D2C-49E5-BF20-CC6184781764","BBA0B61A-8D6C-4B22-93FB-CFB2D9BE4D38","4EB38772-436F-4616-A562-D15961756848","B972A938-F269-4392-A757-D47CF2508FF7","13F6D183-C983-46AE-84D9-D7B55270A54A","A1CD40BA-88CE-4DA3-B4F6-DAD6F149E7D6","CB607A9C-19DB-426E-B763-DC0BB215DE4C","1BC96C4C-A22E-4517-8605-E067CB161C33","953D7BE9-B99F-495B-9A0B-E45DE0EE4310","EDD759B1-374B-4E8A-A582-E4F7D14B0750","65A66293-374F-417F-8695-E991985EDF03","7834B97E-80A4-4CEB-A54E-ECB0AF629AB5","45DDE2CB-1E8F-4E5A-975C-F11DE78650AF","D2D8B8FD-4AA2-4D90-B5D2-F8CE218680A8"],"source":"REAL SNAPSHOT"},{"id":"00000000-0000-0000-0000-000000000003","name":"Citizen","description":"Công dân","isSystemRole":true,"assignedUsers":0,"permissionIds":[],"source":"REAL SNAPSHOT"},{"id":"00000000-0000-0000-0000-000000000002","name":"Officer","description":"Cán b? nghi?p v?","isSystemRole":true,"assignedUsers":0,"permissionIds":[],"source":"REAL SNAPSHOT"}]


const REAL_PERMISSIONS:
  PermissionItem[] =
[{"id":"16434DFC-339C-4F08-B01C-32049E4F5F64","code":"ai.analyze","name":"Phân tích AI","description":"Phân tích AI","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"FD5F052F-B2FB-4C64-9966-812510A9346E","code":"ai.dashboard","name":"Xem Dashboard AI","description":"Xem Dashboard AI","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"ECE05B02-9709-4AE9-9E50-0E08A4539891","code":"ai.scan","name":"Quét d? li?u AI","description":"Quét d? li?u AI","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"C5B650C7-18C3-4A82-8ACD-8138400221AB","code":"ai.updatestatus","name":"C?p nh?t tr?ng thái AI","description":"C?p nh?t tr?ng thái AI","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"6002439C-1D2C-49E5-BF20-CC6184781764","code":"ai.viewrecommendations","name":"Xem khuy?n ngh? AI","description":"Xem khuy?n ngh? AI","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"80420FA3-D9DA-4B07-B91C-AAE5BEED3308","code":"citizens.activate","name":"Kích ho?t công dân","description":"Kích ho?t công dân","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"BBA0B61A-8D6C-4B22-93FB-CFB2D9BE4D38","code":"citizens.create","name":"T?o công dân","description":"T?o công dân","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"EDD759B1-374B-4E8A-A582-E4F7D14B0750","code":"citizens.deactivate","name":"Ng?ng kích ho?t công dân","description":"Ng?ng kích ho?t công dân","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"74497B86-0A4D-4C4E-BB2A-485A51ACAF27","code":"citizens.delete","name":"Xóa công dân","description":"Xóa công dân","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"A7C070B3-C3F7-4297-8B27-1FF31DAB4414","code":"citizens.update","name":"C?p nh?t công dân","description":"C?p nh?t công dân","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"75E6013E-C264-4923-A0F1-19CDA4619BF2","code":"citizens.view","name":"Xem công dân","description":"Xem công dân","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"A67C0DA5-A65A-44C2-AFA0-5A1CDB145FC7","code":"gis.create","name":"T?o d? li?u b?n d?","description":"T?o d? li?u b?n d?","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"7834B97E-80A4-4CEB-A54E-ECB0AF629AB5","code":"gis.delete","name":"Xóa d? li?u b?n d?","description":"Xóa d? li?u b?n d?","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"E5862E66-939A-4819-8801-A61000A54783","code":"gis.read","name":"Xem d? li?u b?n d?","description":"Xem d? li?u b?n d?","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"8496A685-755C-4036-83BD-B0F451394464","code":"gis.update","name":"C?p nh?t d? li?u b?n d?","description":"C?p nh?t d? li?u b?n d?","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"953D7BE9-B99F-495B-9A0B-E45DE0EE4310","code":"households.create","name":"T?o h? gia dình","description":"T?o h? gia dình","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"CB607A9C-19DB-426E-B763-DC0BB215DE4C","code":"households.delete","name":"Xóa h? gia dình","description":"Xóa h? gia dình","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"A1CD40BA-88CE-4DA3-B4F6-DAD6F149E7D6","code":"households.read","name":"Xem h? gia dình","description":"Xem h? gia dình","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"8B1E939B-739F-4CBD-87CA-74D068347EE8","code":"households.update","name":"C?p nh?t h? gia dình","description":"C?p nh?t h? gia dình","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"603CC6E5-0AEF-4DFC-B7A1-C8D35CD36205","code":"map.updatelocation","name":"C?p nh?t v? trí b?n d?","description":"C?p nh?t v? trí b?n d?","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"9E0C4E21-061E-4715-8CDE-18507D7A8BF5","code":"map.view","name":"Xem b?n d?","description":"Xem b?n d?","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"1DCF9282-8925-4500-95FF-21B93C696972","code":"notifications.broadcast","name":"G?i thông báo","description":"G?i thông báo","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"4486116B-3C6D-438B-979B-BE7C3FF306F2","code":"notifications.create","name":"T?o thông báo","description":"T?o thông báo","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"B5479EEA-F849-47FF-A3FF-BC1F9F36CFDD","code":"notifications.read","name":"D?c thông báo","description":"D?c thông báo","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"4EB38772-436F-4616-A562-D15961756848","code":"notifications.retry","name":"G?i l?i thông báo","description":"G?i l?i thông báo","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"16C28C87-2693-43ED-925E-624CA429B94A","code":"notifications.statistics","name":"Xem th?ng kê thông báo","description":"Xem th?ng kê thông báo","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"72D3E121-D262-4BC5-AF7F-956D12C27A74","code":"notifications.view","name":"Xem thông báo","description":"Xem thông báo","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"13F6D183-C983-46AE-84D9-D7B55270A54A","code":"payments.approve","name":"Phê duy?t chi tr?","description":"Phê duy?t chi tr?","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"B7C5D1DC-AF2B-44EA-8B5C-98E7FBE53D74","code":"payments.cancel","name":"H?y chi tr?","description":"H?y chi tr?","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"FEBEDAFB-20E5-405F-96D7-C60196B6EE9E","code":"payments.complete","name":"Hoàn t?t chi tr?","description":"Hoàn t?t chi tr?","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"1BC96C4C-A22E-4517-8605-E067CB161C33","code":"payments.create","name":"T?o chi tr?","description":"T?o chi tr?","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"73DF4888-5248-4511-ADBD-618B4DE0455A","code":"payments.view","name":"Xem chi tr?","description":"Xem chi tr?","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"65A66293-374F-417F-8695-E991985EDF03","code":"permissions.manage","name":"Qu?n ly quy?n h?n","description":"Qu?n ly quy?n h?n","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"A71961F8-C704-4582-B756-9510B95B4DCA","code":"permissions.view","name":"Xem quy?n h?n","description":"Xem quy?n h?n","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"B4588249-8D59-4515-B651-BDD965E9FBC1","code":"roles.create","name":"T?o ch?c danh","description":"T?o ch?c danh","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"2CA51CDF-D31C-428E-907E-BB25E471B676","code":"roles.delete","name":"Xóa ch?c danh","description":"Xóa ch?c danh","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"C5D7A3A2-6461-40D1-917D-512E0EE1B918","code":"roles.update","name":"C?p nh?t ch?c danh","description":"C?p nh?t ch?c danh","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"D6531B08-52FF-4367-8161-4545BEE8C383","code":"roles.view","name":"Xem ch?c danh","description":"Xem ch?c danh","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"A394872C-2266-49B4-A131-688EE3AF1826","code":"system.login","name":"Dang nh?p h? th?ng","description":"Dang nh?p h? th?ng","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"45DDE2CB-1E8F-4E5A-975C-F11DE78650AF","code":"userroles.manage","name":"Qu?n ly phân quy?n ngu?i dùng","description":"Qu?n ly phân quy?n ngu?i dùng","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"1284C282-8DAB-4B6D-B4D7-09B13BF8010B","code":"userroles.view","name":"Xem phân quy?n ngu?i dùng","description":"Xem phân quy?n ngu?i dùng","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"D2D8B8FD-4AA2-4D90-B5D2-F8CE218680A8","code":"welfarecases.cancel","name":"H?y h? so an sinh","description":"H?y h? so an sinh","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"B972A938-F269-4392-A757-D47CF2508FF7","code":"welfarecases.close","name":"Dóng h? so an sinh","description":"Dóng h? so an sinh","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"FAF47852-71C8-417D-9A9E-1409CF5E3597","code":"welfarecases.create","name":"T?o h? so an sinh","description":"T?o h? so an sinh","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"A5D2BFBF-02BD-4F56-AEA1-8805EAA722CA","code":"welfarecases.decide","name":"Ra quy?t d?nh h? so an sinh","description":"Ra quy?t d?nh h? so an sinh","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"D7567A3B-8FD5-4A86-AFB7-60FF9C6815FD","code":"welfarecases.update","name":"C?p nh?t h? so an sinh","description":"C?p nh?t h? so an sinh","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"7D7ADAC4-DDB0-4F73-8ED6-A3D597EA37AF","code":"welfarecases.view","name":"Xem h? so an sinh","description":"Xem h? so an sinh","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"19532748-2935-4866-9230-5B8BEECD692F","code":"welfareprograms.create","name":"T?o chuong trình an sinh","description":"T?o chuong trình an sinh","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"CAA8F0DF-B090-431E-8CB1-01ECB545719A","code":"welfareprograms.update","name":"C?p nh?t chuong trình an sinh","description":"C?p nh?t chuong trình an sinh","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"},{"id":"975C9923-1FCA-4BBA-86CB-7B014E38F078","code":"welfareprograms.view","name":"Xem chuong trình an sinh","description":"Xem chuong trình an sinh","groupId":"00000000-0000-0000-0000-000000000001","groupCode":"SYSTEM","groupName":"H? th?ng"}]


const REAL_GROUPS:
  PermissionGroupItem[] =
[{"id":"00000000-0000-0000-0000-000000000001","code":"SYSTEM","name":"H? th?ng","description":"Qu?n ly h? th?ng"}]


function readDemoRoles():
  RoleItem[] {
  try {
    const raw =
      localStorage.getItem(
        STORAGE_KEY,
      )

    if (!raw)
      return []

    const parsed =
      JSON.parse(raw)

    return Array.isArray(parsed)
      ? parsed as RoleItem[]
      : []
  } catch {
    return []
  }
}


function persistDemoRoles(
  rows:
    RoleItem[],
) {
  localStorage.setItem(
    STORAGE_KEY,
    JSON.stringify(rows),
  )
}


function RolePermissionsPanel() {

  const [
    tab,
    setTab,
  ] =
    useState<ActiveTab>(
      'roles',
    )

  const [
    demoRoles,
    setDemoRoles,
  ] =
    useState<RoleItem[]>(
      () =>
        readDemoRoles(),
    )

  const [
    roleKeyword,
    setRoleKeyword,
  ] =
    useState('')

  const [
    roleSource,
    setRoleSource,
  ] =
    useState('all')

  const [
    permissionKeyword,
    setPermissionKeyword,
  ] =
    useState('')

  const [
    permissionGroup,
    setPermissionGroup,
  ] =
    useState('all')

  const [
    permissionPage,
    setPermissionPage,
  ] =
    useState(1)

  const [
    selectedRole,
    setSelectedRole,
  ] =
    useState<RoleItem | null>(
      null,
    )

  const [
    creating,
    setCreating,
  ] =
    useState(false)

  const [
    form,
    setForm,
  ] =
    useState<RoleForm>({
      name: '',
      description: '',
      permissionIds: [],
    })


  const allRoles =
    useMemo(
      () => [
        ...REAL_ROLES,
        ...demoRoles,
      ],
      [demoRoles],
    )


  const filteredRoles =
    useMemo(
      () => {

        const q =
          roleKeyword
            .trim()
            .toLowerCase()

        return allRoles.filter(
          role => {

            const keywordOk =
              !q ||
              role.name
                .toLowerCase()
                .includes(q) ||
              role.description
                .toLowerCase()
                .includes(q)

            const sourceOk =
              roleSource ===
                'all' ||
              (
                roleSource ===
                  'real' &&
                role.source ===
                  'REAL SNAPSHOT'
              ) ||
              (
                roleSource ===
                  'demo' &&
                role.source ===
                  'DEMO LOCAL'
              )

            return (
              keywordOk &&
              sourceOk
            )
          },
        )
      },
      [
        allRoles,
        roleKeyword,
        roleSource,
      ],
    )


  const filteredPermissions =
    useMemo(
      () => {

        const q =
          permissionKeyword
            .trim()
            .toLowerCase()

        return REAL_PERMISSIONS.filter(
          permission => {

            const keywordOk =
              !q ||
              permission.code
                .toLowerCase()
                .includes(q) ||
              permission.name
                .toLowerCase()
                .includes(q) ||
              permission.description
                .toLowerCase()
                .includes(q)

            const groupOk =
              permissionGroup ===
                'all' ||
              permission.groupId ===
                permissionGroup

            return (
              keywordOk &&
              groupOk
            )
          },
        )
      },
      [
        permissionKeyword,
        permissionGroup,
      ],
    )


  const permissionPages =
    Math.max(
      1,
      Math.ceil(
        filteredPermissions.length /
          PERMISSION_PAGE_SIZE,
      ),
    )


  const currentPermissionPage =
    Math.min(
      permissionPage,
      permissionPages,
    )


  const visiblePermissions =
    filteredPermissions.slice(
      (currentPermissionPage - 1) *
        PERMISSION_PAGE_SIZE,
      currentPermissionPage *
        PERMISSION_PAGE_SIZE,
    )


  const adminRole =
    REAL_ROLES.find(
      role =>
        role.name
          .toLowerCase() ===
          'admin',
    )


  const persistDemo =
    (
      rows:
        RoleItem[],
    ) => {
      setDemoRoles(rows)
      persistDemoRoles(rows)
    }


  const openCreate =
    () => {

      setCreating(true)
      setSelectedRole(null)

      setForm({
        name:
          `DemoRole${demoRoles.length + 1}`,
        description:
          'Vai trò giả lập phục vụ trình diễn phân quyền.',
        permissionIds: [],
      })
    }


  const openRole =
    (
      role:
        RoleItem,
    ) => {

      setCreating(false)
      setSelectedRole(role)

      setForm({
        name:
          role.name,
        description:
          role.description,
        permissionIds:
          [...role.permissionIds],
      })
    }


  const cloneRole =
    (
      role:
        RoleItem,
    ) => {

      const clone:
        RoleItem =
      {
        id:
          `demo-role-${Date.now()}`,
        name:
          `${role.name}_DEMO`,
        description:
          `Bản sao DEMO của ${role.name}.`,
        isSystemRole: false,
        assignedUsers: 0,
        permissionIds:
          [...role.permissionIds],
        source:
          'DEMO LOCAL',
      }

      persistDemo([
        clone,
        ...demoRoles,
      ])

      openRole(clone)
    }


  const closeModal =
    () => {

      setCreating(false)
      setSelectedRole(null)
    }


  const togglePermission =
    (
      permissionId:
        string,
    ) => {

      if (
        selectedRole?.source ===
        'REAL SNAPSHOT'
      ) {
        return
      }

      const exists =
        form.permissionIds.includes(
          permissionId,
        )

      setForm({
        ...form,

        permissionIds:
          exists
            ? form.permissionIds.filter(
                id =>
                  id !==
                  permissionId,
              )
            : [
                ...form.permissionIds,
                permissionId,
              ],
      })
    }


  const selectAllPermissions =
    () => {

      if (
        selectedRole?.source ===
        'REAL SNAPSHOT'
      ) {
        return
      }

      setForm({
        ...form,
        permissionIds:
          REAL_PERMISSIONS.map(
            permission =>
              permission.id,
          ),
      })
    }


  const clearPermissions =
    () => {

      if (
        selectedRole?.source ===
        'REAL SNAPSHOT'
      ) {
        return
      }

      setForm({
        ...form,
        permissionIds: [],
      })
    }


  const saveDemo =
    () => {

      const name =
        form.name.trim()

      if (!name) {
        window.alert(
          'Vui lòng nhập tên vai trò.',
        )
        return
      }

      const duplicate =
        allRoles.some(
          role =>
            role.name
              .toLowerCase() ===
              name.toLowerCase() &&
            role.id !==
              selectedRole?.id,
        )

      if (duplicate) {
        window.alert(
          'Tên vai trò đã tồn tại.',
        )
        return
      }

      if (creating) {

        const item:
          RoleItem =
        {
          id:
            `demo-role-${Date.now()}`,
          name,
          description:
            form.description.trim(),
          isSystemRole: false,
          assignedUsers: 0,
          permissionIds:
            [...form.permissionIds],
          source:
            'DEMO LOCAL',
        }

        persistDemo([
          item,
          ...demoRoles,
        ])

      } else if (
        selectedRole?.source ===
        'DEMO LOCAL'
      ) {

        persistDemo(
          demoRoles.map(
            role =>
              role.id ===
              selectedRole.id
                ? {
                    ...role,
                    name,
                    description:
                      form.description.trim(),
                    permissionIds:
                      [...form.permissionIds],
                  }
                : role,
          ),
        )
      }

      closeModal()
    }


  const deleteDemo =
    (
      role:
        RoleItem,
    ) => {

      if (
        role.source !==
        'DEMO LOCAL'
      ) {
        return
      }

      if (
        !window.confirm(
          `Xóa vai trò DEMO "${role.name}"?`,
        )
      ) {
        return
      }

      persistDemo(
        demoRoles.filter(
          row =>
            row.id !==
            role.id,
        ),
      )

      if (
        selectedRole?.id ===
        role.id
      ) {
        closeModal()
      }
    }


  const selectedIsReal =
    selectedRole?.source ===
    'REAL SNAPSHOT'


  const selectedPermissions =
    selectedRole
      ? REAL_PERMISSIONS.filter(
          permission =>
            selectedRole.permissionIds.includes(
              permission.id,
            ),
        )
      : []


  return (
    <section className="rbac-panel">

      <div className="rbac-heading">

        <div>

          <div className="rbac-eyebrow">
            HỆ THỐNG 02
          </div>

          <h2>
            Vai trò & phân quyền
          </h2>

          <p>
            Ma trận RBAC từ dữ liệu production.
            Role và permission REAL chỉ đọc;
            thay đổi trình diễn được lưu riêng
            dưới dạng DEMO LOCAL.
          </p>

        </div>


        <div className="rbac-heading-actions">

          <button
            type="button"
            className="rbac-button secondary"
            onClick={() => {
              window.location.reload()
            }}
          >
            ↻ Làm mới snapshot
          </button>

          <button
            type="button"
            className="rbac-button primary"
            onClick={openCreate}
          >
            + Tạo vai trò DEMO
          </button>

        </div>

      </div>


      <div className="rbac-safety">

        <strong>
          Bảo vệ RBAC production
        </strong>

        <span>
          Không sửa trực tiếp 3 role và 50 permission REAL.
          Không gỡ quyền Admin production.
          Chức năng tạo/sửa/xóa và gán quyền trong giao diện
          chỉ áp dụng cho DEMO LOCAL.
        </span>

      </div>


      <div className="rbac-stats">

        <article>
          <span>
            Vai trò production
          </span>

          <strong>
            {REAL_ROLES.length}
          </strong>

          <small>
            REAL SNAPSHOT
          </small>
        </article>


        <article>
          <span>
            Permission
          </span>

          <strong>
            {REAL_PERMISSIONS.length}
          </strong>

          <small>
            REAL SNAPSHOT
          </small>
        </article>


        <article>
          <span>
            Nhóm quyền
          </span>

          <strong>
            {REAL_GROUPS.length}
          </strong>

          <small>
            PermissionGroups
          </small>
        </article>


        <article>
          <span>
            Quyền của Admin
          </span>

          <strong>
            {adminRole?.permissionIds.length ?? 0}
          </strong>

          <small>
            RolePermissions
          </small>
        </article>

      </div>


      <div className="rbac-tabs">

        <button
          type="button"
          className={
            tab ===
            'roles'
              ? 'active'
              : ''
          }
          onClick={() =>
            setTab('roles')
          }
        >
          Vai trò
        </button>

        <button
          type="button"
          className={
            tab ===
            'permissions'
              ? 'active'
              : ''
          }
          onClick={() =>
            setTab(
              'permissions',
            )
          }
        >
          Danh sách quyền
        </button>

        <button
          type="button"
          className={
            tab ===
            'matrix'
              ? 'active'
              : ''
          }
          onClick={() =>
            setTab('matrix')
          }
        >
          Ma trận phân quyền
        </button>

      </div>


      {tab ===
        'roles' && (

        <>

          <div className="rbac-toolbar">

            <input
              value={roleKeyword}
              placeholder="Tìm tên hoặc mô tả vai trò..."
              onChange={event =>
                setRoleKeyword(
                  event.target.value,
                )
              }
            />

            <select
              value={roleSource}
              onChange={event =>
                setRoleSource(
                  event.target.value,
                )
              }
            >
              <option value="all">
                Tất cả nguồn
              </option>

              <option value="real">
                REAL SNAPSHOT
              </option>

              <option value="demo">
                DEMO LOCAL
              </option>
            </select>

          </div>


          <div className="rbac-role-grid">

            {filteredRoles.map(
              role => (

                <article
                  className="rbac-role-card"
                  key={role.id}
                >

                  <div className="rbac-role-card-head">

                    <div>
                      <h3>
                        {role.name}
                      </h3>

                      <p>
                        {role.description ||
                          'Không có mô tả.'}
                      </p>
                    </div>

                    <span
                      className={
                        `rbac-source ${
                          role.source ===
                          'REAL SNAPSHOT'
                            ? 'real'
                            : 'demo'
                        }`
                      }
                    >
                      {role.source}
                    </span>

                  </div>


                  <div className="rbac-role-metrics">

                    <div>
                      <span>
                        Quyền
                      </span>

                      <strong>
                        {role.permissionIds.length}
                      </strong>
                    </div>

                    <div>
                      <span>
                        Người dùng
                      </span>

                      <strong>
                        {role.assignedUsers}
                      </strong>
                    </div>

                    <div>
                      <span>
                        Loại
                      </span>

                      <strong>
                        {role.isSystemRole
                          ? 'Hệ thống'
                          : role.source ===
                              'REAL SNAPSHOT'
                            ? 'Nghiệp vụ'
                            : 'DEMO'}
                      </strong>
                    </div>

                  </div>


                  <div className="rbac-role-actions">

                    <button
                      type="button"
                      onClick={() =>
                        openRole(role)
                      }
                    >
                      {role.source ===
                      'REAL SNAPSHOT'
                        ? 'Xem chi tiết'
                        : 'Xem / Sửa'}
                    </button>


                    {role.source ===
                      'REAL SNAPSHOT' ? (

                      <button
                        type="button"
                        onClick={() =>
                          cloneRole(role)
                        }
                      >
                        Sao chép DEMO
                      </button>

                    ) : (

                      <button
                        type="button"
                        className="danger"
                        onClick={() =>
                          deleteDemo(role)
                        }
                      >
                        Xóa
                      </button>
                    )}

                  </div>

                </article>
              ),
            )}

          </div>

        </>
      )}


      {tab ===
        'permissions' && (

        <>

          <div className="rbac-toolbar">

            <input
              value={
                permissionKeyword
              }
              placeholder="Tìm mã, tên hoặc mô tả permission..."
              onChange={event => {
                setPermissionKeyword(
                  event.target.value,
                )
                setPermissionPage(1)
              }}
            />

            <select
              value={
                permissionGroup
              }
              onChange={event => {
                setPermissionGroup(
                  event.target.value,
                )
                setPermissionPage(1)
              }}
            >
              <option value="all">
                Tất cả nhóm quyền
              </option>

              {REAL_GROUPS.map(
                group => (
                  <option
                    value={group.id}
                    key={group.id}
                  >
                    {group.name}
                  </option>
                ),
              )}

            </select>

          </div>


          <div className="rbac-table-wrap">

            <table className="rbac-table">

              <thead>
                <tr>
                  <th>STT</th>
                  <th>Mã permission</th>
                  <th>Tên</th>
                  <th>Mô tả</th>
                  <th>Nhóm quyền</th>
                  <th>Nguồn</th>
                </tr>
              </thead>

              <tbody>

                {visiblePermissions.map(
                  (
                    permission,
                    index,
                  ) => (

                    <tr
                      key={
                        permission.id
                      }
                    >

                      <td>
                        {(currentPermissionPage - 1) *
                          PERMISSION_PAGE_SIZE +
                          index +
                          1}
                      </td>

                      <td>
                        <strong className="rbac-code">
                          {permission.code}
                        </strong>
                      </td>

                      <td>
                        {permission.name}
                      </td>

                      <td>
                        {permission.description ||
                          '—'}
                      </td>

                      <td>
                        {permission.groupName}
                      </td>

                      <td>
                        <span className="rbac-source real">
                          REAL SNAPSHOT
                        </span>
                      </td>

                    </tr>
                  ),
                )}

              </tbody>

            </table>

          </div>


          <div className="rbac-pagination">

            <span>
              {filteredPermissions.length} permission
            </span>

            <div>

              <button
                type="button"
                disabled={
                  currentPermissionPage <=
                  1
                }
                onClick={() =>
                  setPermissionPage(
                    Math.max(
                      1,
                      currentPermissionPage -
                        1,
                    ),
                  )
                }
              >
                ← Trước
              </button>

              <span>
                Trang {currentPermissionPage}/{permissionPages}
              </span>

              <button
                type="button"
                disabled={
                  currentPermissionPage >=
                  permissionPages
                }
                onClick={() =>
                  setPermissionPage(
                    Math.min(
                      permissionPages,
                      currentPermissionPage +
                        1,
                    ),
                  )
                }
              >
                Sau →
              </button>

            </div>

          </div>

        </>
      )}


      {tab ===
        'matrix' && (

        <>

          <div className="rbac-matrix-info">

            <div>
              <h3>
                Ma trận quyền production
              </h3>

              <p>
                Dấu ✓ cho biết role đang sở hữu permission
                trong snapshot production.
              </p>
            </div>

            <span>
              {REAL_PERMISSIONS.length} quyền × {REAL_ROLES.length} role
            </span>

          </div>


          <div className="rbac-table-wrap">

            <table className="rbac-table rbac-matrix-table">

              <thead>
                <tr>
                  <th>Permission</th>

                  {REAL_ROLES.map(
                    role => (
                      <th key={role.id}>
                        {role.name}
                      </th>
                    ),
                  )}

                </tr>
              </thead>


              <tbody>

                {REAL_PERMISSIONS.map(
                  permission => (

                    <tr
                      key={
                        permission.id
                      }
                    >

                      <td>

                        <strong className="rbac-code">
                          {permission.code}
                        </strong>

                        <small>
                          {permission.name}
                        </small>

                      </td>


                      {REAL_ROLES.map(
                        role => {

                          const checked =
                            role.permissionIds.includes(
                              permission.id,
                            )

                          return (
                            <td
                              key={
                                role.id
                              }
                              className="rbac-matrix-cell"
                            >
                              <span
                                className={
                                  checked
                                    ? 'granted'
                                    : 'denied'
                                }
                              >
                                {checked
                                  ? '✓'
                                  : '—'}
                              </span>
                            </td>
                          )
                        },
                      )}

                    </tr>
                  ),
                )}

              </tbody>

            </table>

          </div>

        </>
      )}


      {(creating ||
        selectedRole) && (

        <div className="rbac-modal-overlay">

          <div className="rbac-modal">

            <div className="rbac-modal-heading">

              <div>

                <h3>
                  {creating
                    ? 'Tạo vai trò DEMO'
                    : selectedIsReal
                      ? `Chi tiết role ${selectedRole?.name ?? ''}`
                      : `Chỉnh sửa ${selectedRole?.name ?? ''}`}
                </h3>

                <p>
                  {selectedIsReal
                    ? 'Role production chỉ đọc.'
                    : 'Thay đổi chỉ lưu trong localStorage.'}
                </p>

              </div>

              <button
                type="button"
                className="rbac-modal-close"
                onClick={closeModal}
              >
                ×
              </button>

            </div>


            <div className="rbac-form-grid">

              <label>
                <span>
                  Tên vai trò
                </span>

                <input
                  disabled={
                    selectedIsReal
                  }
                  value={form.name}
                  onChange={event =>
                    setForm({
                      ...form,
                      name:
                        event.target.value,
                    })
                  }
                />
              </label>


              <label className="full">
                <span>
                  Mô tả
                </span>

                <textarea
                  disabled={
                    selectedIsReal
                  }
                  value={
                    form.description
                  }
                  onChange={event =>
                    setForm({
                      ...form,
                      description:
                        event.target.value,
                    })
                  }
                />
              </label>

            </div>


            <div className="rbac-permission-editor-head">

              <div>
                <h4>
                  Permission
                </h4>

                <p>
                  {selectedIsReal
                    ? `${selectedPermissions.length} quyền production`
                    : `${form.permissionIds.length} quyền được chọn`}
                </p>
              </div>


              {!selectedIsReal && (

                <div>

                  <button
                    type="button"
                    onClick={
                      selectAllPermissions
                    }
                  >
                    Chọn tất cả
                  </button>

                  <button
                    type="button"
                    onClick={
                      clearPermissions
                    }
                  >
                    Bỏ chọn
                  </button>

                </div>
              )}

            </div>


            <div className="rbac-permission-editor">

              {REAL_PERMISSIONS.map(
                permission => {

                  const checked =
                    selectedIsReal
                      ? (
                          selectedRole
                            ?.permissionIds
                            .includes(
                              permission.id,
                            ) ??
                          false
                        )
                      : form.permissionIds.includes(
                          permission.id,
                        )

                  return (

                    <label
                      className={
                        checked
                          ? 'selected'
                          : ''
                      }
                      key={
                        permission.id
                      }
                    >

                      <input
                        type="checkbox"
                        disabled={
                          selectedIsReal
                        }
                        checked={checked}
                        onChange={() =>
                          togglePermission(
                            permission.id,
                          )
                        }
                      />

                      <div>

                        <strong>
                          {permission.code}
                        </strong>

                        <span>
                          {permission.name}
                        </span>

                      </div>

                    </label>
                  )
                },
              )}

            </div>


            <div className="rbac-modal-actions">

              <button
                type="button"
                className="rbac-button secondary"
                onClick={closeModal}
              >
                Đóng
              </button>


              {selectedIsReal &&
                selectedRole && (

                <button
                  type="button"
                  className="rbac-button secondary"
                  onClick={() =>
                    cloneRole(
                      selectedRole,
                    )
                  }
                >
                  Sao chép thành DEMO
                </button>
              )}


              {!selectedIsReal && (

                <button
                  type="button"
                  className="rbac-button primary"
                  onClick={saveDemo}
                >
                  Lưu vai trò DEMO
                </button>
              )}

            </div>

          </div>

        </div>
      )}

    </section>
  )
}

export default RolePermissionsPanel