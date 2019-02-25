$("#btn_import").fileinput({
    language: 'zh', //设置语言
    uploadUrl: "/Import/Import", //上传的地址
    allowedFileExtensions: ['xls', 'xlsx'],//接收的文件后缀
    //uploadExtraData: { "filename": $("#uploadfile").val() },
    uploadAsync: true, //默认异步上传
    showUpload: true, //是否显示上传按钮
    showRemove: false, //显示移除按钮
    showPreview: true, //是否显示预览
    showCaption: false,//是否显示标题
    browseClass: "btn btn-primary", //按钮样式     
    dropZoneEnabled: false,//是否显示拖拽区域
    //minImageWidth: 50, //图片的最小宽度
    //minImageHeight: 50,//图片的最小高度
    //maxImageWidth: 1000,//图片的最大宽度
    //maxImageHeight: 1000,//图片的最大高度
    //maxFileSize: 0,//单位为kb，如果为0表示不限制文件大小
    //minFileCount: 0,
    maxFileCount: 1, //表示允许同时上传的最大文件个数
    enctype: 'multipart/form-data',
    validateInitialCount: true,
    previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",
    msgFilesTooMany: "选择上传的文件数量({n}) 超过允许的最大数值{m}！"
}).on("fileuploaded", function (event, data) {

});

$("#btn_import").on("fileuploaded", function (event, data, previewId, index) {
    if (data.response.Status) {
        $.messager.alert("Info", "Import successfully", "info");
    }
    else {
        $.messager.alert("Alert", data.response.Message, "info");
    }
});


$(function () {
    operate.operateInit();
    tableInit.Init();
})

var tableInit = {
    Init: function () {
        //searchOnEnterKey 是 true时  enter会触发这列的排序功能，从而影响查询
        //绑定table的viewmodel
        this.myViewModel = new ko.bootstrapTableViewModel({
            url: '/Home/GetList',
            method: 'get',
            toolbar: '#toolbar',
            striped: true,
            cache: false,
            pagination: true,
            sortable: true,
            sortOrder: "desc",
            queryParams: function (params) {
                var temp = {
                    limit: params.limit,
                    offset: params.offset,
                    filter: params.filter,
                    sort: params.sort,
                    order: params.order,
                };
                return temp;
            },
            sidePagination: "server",
            pageNumber: 1,
            pageSize: 20,                       //每页的记录行数（*）
            pageList: [20, 50, 100, 200],
            search: false,
            singleSelect: true,
            searchOnEnterKey: false,
            showColumns: true,
            showRefresh: true,
            minimumCountColumns: 2,
            clickToSelect: true,
            uniqueId: "Id",
            height: 500,
            cardView: false,
            detailView: false,
            //showExport: true,                   
            //exportDataType: "basic",
            filterControl: true,
            onDblClickRow: function (row, $element) {
                $("#ViewModal").modal().on("shown.bs.modal", function () {
                    $.ajax({
                        url: "/Home/GetDetailTableHtml",
                        type: "get",
                        data: {
                            projectId: row.Id,
                        },
                        contentType: 'application/json',
                        // 回调函数，接受服务器端返回给客户端的值，即result值
                        success: function (data) {
                            var obj = JSON.parse(data);
                            $.each(obj, function (index, value) {
                                if (value.flag == "order") {
                                    $('.order .flag').html(value.html);
                                }
                                if (value.flag == "income") {
                                    $('.income .flag').html(value.html);
                                }
                            });
                        },
                        error: function (data) {

                        }
                    })

                }).on('hidden.bs.modal', function () {
                });
            },
            columns: [{
                checkbox: true
            },
            {
                field: 'Id',
                title: 'Id',
                visible: false,
                switchable: false
            },
             {
                 field: 'AccountName',
                 title: 'Account Name',
                 switchable: false,
                 filterControl: 'input'
             },
               {
                   field: 'OpportunityOwner',
                   title: 'OpportunityOwner',
                   sortable: true,
                   filterControl: 'input'
               },
                  {
                      field: 'Territory',
                      title: 'Territory',
                      filterControl: 'input'
                  },
                {
                    field: 'QuoteNumber',
                    title: 'QuoteNumber',
                    sortable: true,
                    filterControl: 'input'
                },
                 {
                     field: 'StudySite',
                     title: 'StudySite',
                     sortable: true,
                     filterControl: 'input'
                 },
                   {
                       field: 'ProjectLine',
                       title: 'ProjectLine',
                       sortable: true,
                       filterControl: 'input'
                   },
                     {
                         field: 'TotalBooking',
                         title: 'TotalBooking',
                         sortable: true,
                     },
                       {
                           field: 'KickOffDate',
                           title: 'KickOffDate',
                           sortable: true,
                           filterControl: 'input'
                       },
                         {
                             field: 'Status',
                             title: 'Status',
                             sortable: true,
                             filterControl: 'input'
                         },
                          {
                              field: 'ProjectClosedDate',
                              title: 'ProjectClosedDate',
                              sortable: true,
                              filterControl: 'input'
                          },
                           {
                               field: 'USDCurrency',
                               title: 'USDCurrency',
                           },
                            {
                                field: 'FinalCost',
                                title: 'FinalCost',
                                sortable: true,
                            },
                              {
                                  field: 'different',
                                  title: 'different',
                                  sortable: true,
                              },
            ],
        });
        ko.applyBindings(this.myViewModel, document.getElementById("table"));
    }
};

var operate = {
    //初始化按钮事件
    operateInit: function () {
        this.operateAdd();
    },
    operateAdd: function () {

    }
}


