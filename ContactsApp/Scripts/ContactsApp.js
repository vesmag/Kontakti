var ContactsApp = angular.module("ContactsApp", ['ngRoute']);


ContactsApp.config(function ($routeProvider) {
    $routeProvider
         .when('/all', {
             controller: 'allController',
             templateUrl: 'Partials/All'
         })
        .when('/contact/:id', {
            controller: 'contactController',
            templateUrl: 'Partials/Contact'
        })
});