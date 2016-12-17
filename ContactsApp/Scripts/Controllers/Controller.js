var myController = function ($scope, myService) {
    GetAllContacts();

    function GetAllContacts() {
        var contactsData = myService.get_Contacts();
        contactsData.then(function successCallback(response) {
            $scope.contacts = response.data;
        }, function errorCallback(response) {
            return "Error";
        });
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
    
    $scope.searchContact = function (searchOption, searchString) {
        switch (searchOption) {
            case "SearchByName":
                var searching = myService.name_Search(searchString);
                break;
            case "SearchBySurname":
                var searching = myService.surname_Search(searchString);
                break;
            case "SearchByTag":
                var searching = myService.tag_Search(searchString);
                break;
            default:
                console.log("Error: search option not specified");
        }
        searching.then(function successCallback(response) {
            $scope.filteredContacts = response.data;
            console.log($scope.filteredContacts);
        }, function errorCallback(response) {
            return "Error";
        });
    }

    $scope.addContact = function (newContact) {
        console.log(newContact);
        var adding = myService.add_Contact(newContact);
        adding.then(function successCallback(response) {
            console.log(response);
            $scope.contacts = response.data;
        }, function errorCallback(response) {
            return "Error";
        });
        $scope.contacts.push(newContact);
    }

    $scope.createDB = function () {
        console.log("usao");
        var creating = myService.create_DB();
        creating.then(function successCallback(response) {
            console.log(response);
        }, function errorCallback(response) {
            return "Error";
        });
    }
}

myController.$inject = ['$scope', 'myService'];