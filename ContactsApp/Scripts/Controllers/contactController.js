ContactsApp.controller("contactController", function ($scope, $rootScope, $routeParams, $location, myService) {
    //funckije u ovom controlleru uglavnom zovu myService koji salje odgovarajuci $http request na bazu

    //na osnovu id-a, funckija iz baze vraca sve podatke jednog kontakta
    function getContactById(id) {
        var getting = myService.get_Contact_By_Id(id);
        getting.then(function successCallback(response) {
            $rootScope.Name = response.data.Name;
            $rootScope.Surname = response.data.Surname;
            $rootScope.Address = response.data.Address;
            $rootScope.Email = response.data.Email;
            $rootScope.Telephone = response.data.Telephone;
            $rootScope.Tags = response.data.Tags;
        }, function errorCallback(response) {
            return "Error";
        });
    }

    //init funckija se poziva svakim ucitavanjem "Contact" viewa
    //uzima id iz URL-a i poziva funkciju getContactById
    function init() {
        var contactId = $location.path().split("/")[2];
        getContactById(contactId);
    }

    init();

});