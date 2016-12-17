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

}

myController.$inject = ['$scope', 'myService'];