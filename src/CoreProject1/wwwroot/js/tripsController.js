(function() {
    'use strict';

    angular.module("app-trips")
        .controller("tripsController", tripsController);

    function tripsController($http) {
        var vm = this;
        vm.name = "test";
        vm.trips = [
            {
                name: "US Trip",
                created: new Date()
            }, {
                name: "World Trip",
                created: new Date()
            }
        ];

        vm.newTrip = {};
        vm.errorMessage = "";
        vm.isBusy = true;

        //-- Retrieving data
        $http.get("/api/trips")
            .then(function(response) {
                //success
                angular.copy(response.data, vm.trips);
                },
                function(error) {
                    //failure
                    vm.errorMessage = "Failed to load data: " + error;
                })
        .finally(function() {
                vm.isBusy = false;
        });

        //-- Creating data
        vm.addTrip = function() {
            vm.isBusy = true;
            vm.errorMessage = "";
            $http.post("/api/trips", vm.newTrip)
                .then(function(response) {
                    //-- success
                        //alert("added");
                    vm.trips.push(response.data);
                        vm.newTrip = {};
                    },
                    function() {
                        //-- error
                        vm.errorMessage = "Failed to save new trip";
                    })
                .finally(function() {
                    vm.isBusy = false;
                });
        };

    }


})();