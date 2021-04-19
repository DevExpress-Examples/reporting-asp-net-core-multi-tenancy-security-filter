function InitDesigner(args) {
    var designer = args.designerModel;
    $(window).on('beforeunload', function (e) {
        if (designer.isDirty()) {
            designer.navigateByReports.closeAll().done(function () {
                return;
            });
            return "There are unsaved changes in the designer.";
        }
    });
}

function ExitDesigner() {
    window.location = "/";
}
