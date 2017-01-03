ContactsApp.controller("contactController", function ($scope, $rootScope, $routeParams, $location, myService) {
    //funckije u ovom controlleru uglavnom zovu myService koji salje odgovarajuci $http request na bazu

    //na osnovu id-a, funckija iz baze vraca sve podatke jednog kontakta
    function getContactById(id) {
        var getting = myService.get_Contact_By_Id(id);
        getting.then(function successCallback(response) {
            console.log(response.data);
            $rootScope.Name = response.data.Name;
            $rootScope.Surname = response.data.Surname;
            $rootScope.Address = response.data.Address;
            $rootScope.Email = response.data.Emails;
            $rootScope.Telephone = response.data.Telephones;
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

    $scope.getDetails = function (contact) {
        $rootScope.Name = contact.Name;
        $rootScope.Surname = contact.Surname;
        $rootScope.Address = contact.Address;
        $rootScope.Email = contact.Email;
        $rootScope.Telephone = contact.Telephone;
        $rootScope.Tags = contact.Tags;
        var path = $location.path().split("/")[0];
        $location.path(path + 'contact/' + contact.Id);
    }


    $scope.deleteContact = function (Con) {
        //deletanje rowa iz tablice (dok se još izvršava $http)
        var newList = [];
        angular.forEach($scope.contacts, function (contact) {
            if (contact.Id != Con)
                newList.push(contact);
            $scope.contacts = newList;
        })

        var deleting = myService.delete_Contact(Con);
        deleting.then(function successCallback(response) {
            console.log(response);
        }, function errorCallback(response) {
            return "Error";
        });
    }

    $scope.updateContact = function (Con) {
        console.log(Con);
        var updating = myService.update_Contact(Con);
        updating.then(function successCallback(response) {
            console.log(response);
        }, function errorCallback(response) {
            return "Error";
        });
    }
});