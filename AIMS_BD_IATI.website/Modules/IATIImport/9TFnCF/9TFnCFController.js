﻿angular.module('iatiDataImporter').controller("9TFnCFController", function ($rootScope, $uibModalInstance, $timeout, $scope, $http, Activity, Project) {
    $scope.Activity = Activity;
    $scope.Project = Project;

    $http({
        method: 'GET',
        url: apiprefix + '/api/IATIImport/GetTrustFundDetails',
        params: { trustFundId: Activity.MappedTrustFundId }
    }).success(function (result) {
        $scope.TrustFundDetails = result;
    });


    $scope.ok = function () {
        $uibModalInstance.close();
    };


    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});
