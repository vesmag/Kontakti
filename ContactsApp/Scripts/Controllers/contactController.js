ContactsApp.controller("contactController", function ($scope, $rootScope, $routeParams, $location, myService) {
    $scope.editMailsMode = false;
    //funckije u ovom controlleru uglavnom zovu myService koji salje odgovarajuci $http request na bazu

    //na osnovu id-a, funckija iz baze vraca sve podatke jednog kontakta
    function getContactById(id) {
        var getting = myService.get_Contact_By_Id(id);
        getting.then(function successCallback(response) {
            $rootScope.Id = response.data.Id;
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
    /*
    $scope.getDetails = function (contact) {
        $rootScope.Name = contact.Name;
        $rootScope.Surname = contact.Surname;
        $rootScope.Address = contact.Address;
        $rootScope.Email = contact.Email;
        $rootScope.Telephone = contact.Telephone;
        $rootScope.Tags = contact.Tags;
        var path = $location.path().split("/")[0];
        $location.path(path + 'contact/' + contact.Id);
    }*/

    /*
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
    }*/

    //funkcije za deletanje: primaju instancu klase Email i po Email.EntryIdu brisu iz $rootScope.Email te zovu fje za 
    //brisanje tog emaila u bazi
    $scope.deleteEmail = function (Em) {
        //deletanje iz liste emailova (dok se još izvršava $http)
        var newList = [];
        angular.forEach($rootScope.Email, function (email) {
            if (email.EntryId != Em.EntryId)
                newList.push(email);
            $rootScope.Email = newList;
        })
        var deleting = myService.delete_Email(Em);
        deleting.then(function successCallback(response) {
            return response;
        }, function errorCallback(response) {
            return "Error";
        });
    }

    $scope.deleteTelephone = function (Tel) {
        //deletanje iz liste telefona (dok se još izvršava $http)
        var newList = [];
        angular.forEach($rootScope.Telephone, function (telephone) {
            if (telephone.EntryId != Tel.EntryId)
                newList.push(telephone);
            $rootScope.Telephone = newList;
        })
        var deleting = myService.delete_Telephone(Tel);
        deleting.then(function successCallback(response) {
            return response;
        }, function errorCallback(response) {
            return "Error";
        });
    }

    $scope.deleteTag = function (Tag) {
        //deletanje iz liste tagova (dok se još izvršava $http)
        var newList = [];
        angular.forEach($rootScope.Tags, function (tag) {
            if (tag.EntryId != Tag.EntryId)
                newList.push(tag);
            $rootScope.Tags = newList;
        })
        var deleting = myService.delete_Tag(Tag);
        deleting.then(function successCallback(response) {
            return response;
        }, function errorCallback(response) {
            return "Error";
        });
    }

    //funkcije za update imena, prezimena i adrese: primaju id osobe i string koji treba unijeti umjesto trenutnog 
    //imena/prezimena/adrese u bazu
    $scope.updateName = function (Id, Name) {
        var updating = myService.update_Name(Id, Name);
        updating.then(function successCallback(response) {
            return response;
        }, function errorCallback(response) {
            return "Error";
        });
    }

    $scope.updateSurname = function (Id, Surname) {
        var updating = myService.update_Surname(Id, Surname);
        updating.then(function successCallback(response) {
            return response;
        }, function errorCallback(response) {
            return "Error";
        });
    }

    $scope.updateAddress = function (Id, Address) {
        var updating = myService.update_Address(Id, Address);
        updating.then(function successCallback(response) {
            return response;
        }, function errorCallback(response) {
            return "Error";
        });
    }

    //fje za update emaila/tel.broja/taga: primaju instancu klase Email/Telephone/Tag i salju serviceu koji zove
    //fje za update baze
    $scope.updateEmail = function (Em) {
        if (Em == undefined) return;
        var updating = myService.update_Email(Em);
        updating.then(function successCallback(response) {
            return response;
        }, function errorCallback(response) {
            return "Error";
        });
    }

    $scope.updateTelephone = function (Tel) {
        if (Tel == undefined) return;
        var updating = myService.update_Telephone(Tel);
        updating.then(function successCallback(response) {
            return response;
        }, function errorCallback(response) {
            return "Error";
        });
    }

    $scope.updateTag = function (Tag) {
        if (Tag == undefined) return;
        var updating = myService.update_Tag(Tag);
        updating.then(function successCallback(response) {
            return response;
        }, function errorCallback(response) {
            return "Error";
        });
    }

    $scope.addEmail = function (Id, Email) {
        //ne primamo prazan email
        if (Email == undefined) return;

        var Em = { PersonId: Id, Email1: Email }
        //novi email pusha se u $rootScope.Email kako bi se korisniku sakrila asinkronost
        $rootScope.Email.push(Em);
        var adding = myService.add_Email(Em);
        adding.then(function successCallback(response) {
            Em.EntryId = response.data;
            return response;
        }, function errorCallback(response) {
            return "Error";
        });
        //postavljamo emailToAdd na prazan string kako se u unosu sljedeceg novog emaila ne bi pojavio zadnji uneseni
        $scope.emailToAdd = "";
    }

    //funkcija analogna addEmail za dodavanje tel.broja
    $scope.addTelephone = function (Id, Telephone) {
        if (Telephone == undefined) return;
        var Tel = { PersonId: Id, Telephone1: Telephone }
        $rootScope.Telephone.push(Tel);
        var adding = myService.add_Telephone(Tel);
        adding.then(function successCallback(response) {
            Tel.EntryId = response.data;
            return response;
        }, function errorCallback(response) {
            return "Error";
        });
        $scope.telephoneToAdd = "";
    }

    //funkcija analogna addEmail za dodavanje taga
    $scope.addTag = function (Id, Tag) {
        if (Tag == undefined) return;
        var Tag = { PersonId: Id, Tag1: Tag }
        $rootScope.Tags.push(Tag);
        var adding = myService.add_Tag(Tag);
        adding.then(function successCallback(response) {
            Tag.EntryId = response.data;
            return response;
        }, function errorCallback(response) {
            return "Error";
        });
        $scope.tagToAdd = "";
    }
});