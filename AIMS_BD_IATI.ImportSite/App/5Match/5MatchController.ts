﻿/// <reference path="../../scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../../Modules/authentication/definitions.d.ts" />
/// <reference path="../IatiImportApp.ts" />


angular.module('iatiDataImporter').controller("5MatchController", function ($rootScope: RootScopeModel, $scope, $http, $timeout) {
    $scope.models = $rootScope.models;
    //$('#divView').slimScroll({ scrollTo: '0px' });

    $scope.Commands = {
        saveData: function () {
            $http({
                url: apiprefix + '/api/IATIImport/SubmitManualMatching',
                method: 'POST',
                data: JSON.stringify($scope.models),
                dataType: 'json'
            }).then(function (result) {
                $timeout(function () {
                    document.getElementById('btn6GeneralPreferences').click(); //redirect
                });

                //deferred.resolve(result);
            },
            function (response) {
                //deferred.reject(response);
            });

        }

    };


});
