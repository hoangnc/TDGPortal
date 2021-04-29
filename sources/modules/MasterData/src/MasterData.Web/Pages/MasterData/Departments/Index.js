(function () {
    var l = abp.localization.getResource('MasterData');
    var _departmentAppService = masterData.departments.department;

    var _editModal = new abp.ModalManager(
        abp.appPath + 'MasterData/Departments/EditModal'
    );
    var _createModal = new abp.ModalManager(
        abp.appPath + 'MasterData/Departments/CreateModal'
    );

    var _dataTable = null;

    abp.ui.extensions.entityActions.get('masterData.department').addContributor(
        function(actionList) {
            return actionList.addManyTail(
                [
                    {
                        text: l('Edit'),
                        visible: abp.auth.isGranted(
                            'MasterData.Departments.Update'
                        ),
                        action: function (data) {
                            _editModal.open({
                                id: data.record.id,
                            });
                        },
                    },
                    {
                        text: l('Delete'),
                        visible: abp.auth.isGranted(
                            'MasterData.Departments.Delete'
                        ),
                        confirmMessage: function (data) {
                            return l(
                                'CompanyDeletionConfirmationMessage',
                                data.record.name
                            );
                        },
                        action: function (data) {
                            _departmentAppService
                                .delete(data.record.id)
                                .then(function () {
                                    _dataTable.ajax.reload();
                                });
                        },
                    }
                ]
            );
        }
    );

    abp.ui.extensions.tableColumns.get('masterData.department').addContributor(
        function (columnList) {
            columnList.addManyTail(
                [
                    {
                        title: l("Actions"),
                        rowAction: {
                            items: abp.ui.extensions.entityActions.get('masterData.department').actions.toArray()
                        }
                    },
                    {
                        title: l("Display:DepartmentCode"),
                        data: 'code'
                    },
                    {
                        title: l("Display:DepartmentName"),
                        data: 'name',
                    },
                    {
                        title: l("Display:DepartmentEmailAddress"),
                        data: 'emailAddress',
                    }
                ]
            );
        },
        0 //adds as the first contributor
    );

    $(function () {
        const _$wrapper = $('#DepartmentsWrapper');

        _dataTable = _$wrapper.find('table').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                order: [[1, 'asc']],
                processing: true,
                paging: true,
                scrollX: true,
                serverSide: true,
                ajax: abp.libs.datatables.createAjax(_departmentAppService.getList),
                columnDefs: abp.ui.extensions.tableColumns.get('masterData.department').columns.toArray(),
            })
        );

        _createModal.onResult(function () {
            _dataTable.ajax.reload();
        });

        _editModal.onResult(function () {
            _dataTable.ajax.reload();
        });

        _$wrapper.find('button[name=CreateDepartment]').click(function (e) {
            e.preventDefault();
            _createModal.open();
        });
    });
})();
