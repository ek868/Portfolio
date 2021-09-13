from data import *
from tokens import *
from global_scope_functions import is_OwnerAdmin_Member


def admin_userpermission_change(token, u_id, permission_id):
    target = getUserDetails(u_id)

    if not permission_id == 1 or not permission_id == 2 or not permission_id == 3:
        raise ValueError("Invalid permission_id")

    connections = getConnections()
    if token not in connections:
        raise ValueError("token not logged in")

    origin = getUserDetails(getIdFromToken(token))

    if not is_OwnerAdmin_Member(origin.id):
        raise AccessError("operation not permitted: Not an owner or admin")

    if origin.permission == 2 and target.permission == 1:
        raise AccessError("operation not permitted: Admin cannot change owner")

    previous_permission = target.permission
    target.permission = permission_id
    setUserDetails(target)


    # remove from previous dic
    if previous_permission == 3:
        for i in range(len(members_dictionaries)):
            if members_dictionaries[i]['u_id'] == target.id:
                del members_dictionaries[i]
    if previous_permission == 2:
        for i in range(len(admins_dictionaries)):
            if admins_dictionaries[i]['u_id'] == target.id:
                del admins_dictionaries[i]
    if previous_permission == 1:
        for i in range(len(owners_dictionaries)):
            if owners_dictionaries[i]['u_id'] == target.id:
                del owners_dictionaries[i]

    # add to target dic
    if permission_id == 3:
        members_dictionaries.append({'u_id':target.id, 'name_first':target.firstName, 'name_last':target.lastName})
    if permission_id == 2:
        admins_dictionaries.append({'u_id':target.id, 'name_first':target.firstName, 'name_last':target.lastName})
    if permission_id == 1:

        owners_dictionaries.append({'u_id':target.id, 'name_first':target.firstName, 'name_last':target.lastName})
