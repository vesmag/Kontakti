ContactsApp.controller("allController", function ($scope, $rootScope, $routeParams, $location, myService) {
    //sve funckije u ovom controlleru zovu pripadajucu funkciju u myService koja komunicira s bazom

    //prilikom ucitavanja "All" viewa, treba dohvatiti sve kontakte iz baze
    GetAllContacts();

    function GetAllContacts() {
        var contactsData = myService.get_Contacts();
        contactsData.then(function successCallback(response) {
            $scope.contacts = response.data;
        }, function errorCallback(response) {
            return "Error";
        });
    }



    //sprema podatke iz kontakta koji je pozvao fju u rootScope koji ce contact view citati
    $scope.getDetails = function (contact) {
        $rootScope.Name = contact.Name;
        $rootScope.Surname = contact.Surname;
        $rootScope.Address = contact.Address;
        $rootScope.Email = contact.Email;
        $rootScope.Telephone = contact.Telephone;
        $rootScope.Tags = contact.Tags;

        //path mijenjamo na /contact/id
        var path = $location.path().split("/")[0];
        $location.path(path + 'contact/' + contact.Id);
    }

    $scope.deleteContact = function (Con) {
        console.log("delete");
        console.log(Con);
        //prvo vrsimo brisanje retka iz tablice (dok se još izvrsava $http), kako bi se korisniku sakrila asinkronost
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
    /*
    //funkcija koja se poziva prilikom editiranja kontakta
    $scope.updateContact = function (Con) {
        var updating = myService.update_Contact(Con);
        updating.then(function successCallback(response) {
            console.log(response);
        }, function errorCallback(response) {
            return "Error";
        });
    }

    //funkcija zove funkciju za trazenje searchStringa u imenu, prezimenu ili tagu kontakata
    $scope.search = function (searchString) {
        var searching = myService.search(searchString);
        searching.then(function successCallback(response) {
            $scope.filteredContacts = response.data;
            console.log($scope.filteredContacts);
        }, function errorCallback(response) {
            return "Error";
        });
    }*/

    //dodavanje novog kontakta, dodajemo ga u bazu i pushamo u $scope.contacts
    $scope.addContact = function (newContact) {
        var mails = newContact.Email.split(" ");
        var tels = newContact.Telephone.split(" ");
        var tags = newContact.Tags.split(" ");
        $scope.contacts.push({
            Name: newContact.Name,
            Surname: newContact.Surname,
            Address: newContact.Address,
            Emails: mails,
            Telephones: tels,
            Tags: tags
        });
        var adding = myService.add_Contact(newContact);
        adding.then(function successCallback(response) {
            mails.forEach(function (mail) {
                var Em = { PersonId: response.data, Email1: mail }
                var adding2 = myService.add_Email(Em);
                adding2.then(function successCallback(response) {
                    console.log(response);
                }, function errorCallback(response) {
                    return "Error";
                });
            });

            tels.forEach(function (tel) {
                var Tel = { PersonId: response.data, Telephone1: tel }
                var adding2 = myService.add_Telephone(Tel);
                adding2.then(function successCallback(response) {
                    console.log(response);
                }, function errorCallback(response) {
                    return "Error";
                });
            })
            
            tags.forEach(function (tag) {
                var Tag = { PersonId: response.data, Tag1: tag }
                var adding2 = myService.add_Tag(Tag);
                adding2.then(function successCallback(response) {
                    console.log(response);
                }, function errorCallback(response) {
                    return "Error";
                });
            })

            console.log(response);

        }, function errorCallback(response) {
            return "Error";
        });
        $scope.newContact = {};
    }

    //funkcija za stvaranje baze - ne koristi se u kodu
    /*$scope.createDB = function () {
        var creating = myService.create_DB();
        creating.then(function successCallback(response) {
            console.log(response);
        }, function errorCallback(response) {
            return "Error";
        });
    }*/
});