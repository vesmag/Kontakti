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
        $rootScope.Id = contact.Id;
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

    //brisanje kontakta iz baze, argument je id korisnika kojeg treba obrisati
    $scope.deleteContact = function (Con) {
        //prvo vrsimo brisanje retka iz tablice (dok se još izvrsava $http), kako bi se korisniku sakrila asinkronost
        var newList = [];
        angular.forEach($scope.contacts, function (contact) {
            if (contact.Id != Con)
                newList.push(contact);
            $scope.contacts = newList;
        })

        var deleting = myService.delete_Contact(Con);
        deleting.then(function successCallback(response) {
            console.log(response.data);
        }, function errorCallback(response) {
            return "Error";
        });
    }
        
    //funkcija zove funkciju za trazenje searchStringa u imenu, prezimenu ili tagu kontakata
    $scope.search = function (searchString) {
        var searching = myService.search(searchString);
        searching.then(function successCallback(response) {
            $scope.filteredContacts = response.data;
        }, function errorCallback(response) {
            return "Error";
        });
    }

    //dodavanje novog kontakta, dodajemo ga u bazu i pushamo u $scope.contacts
    $scope.addContact = function (newContact) {
        //ako je nista nije uneseno niti u jednu od kucica, nista ne radimo
        if (newContact == undefined) return;

        //emailove, tel. brojeve i tagove razdvajamo po zarezima i spremamo u array pojedini email/tel.broj/tag
        //ako je ijedna od kucica u unosu ostavljena prazna, preskacemo ju
        var emails = new Array();
        var telephones = new Array();
        var tags = new Array();
        if (newContact.Email != undefined) {
            var tempEmails = newContact.Email.split(",");
            tempEmails.forEach(function (email) {
                emails.push({
                    Email1: email
                });
            });
        }
        if (newContact.Telephone != undefined) {
            var tempTels = newContact.Telephone.split(",");
            tempTels.forEach(function (tel) {
                telephones.push({
                    Telephone1: tel
                });
            });
        }
        if (newContact.Tags != undefined) {
            var tempTags = newContact.Tags.split(",");
            tempTags.forEach(function (tag) {
                tags.push({
                    Tag1: tag
                });
            });
        }

        var adding = myService.add_Contact(newContact);
        adding.then(function successCallback(response) {
            //novi kontakt pushamo u $scope.contacts kako bi se korisniku sakrila asinkronost
            $scope.contacts.push({
                Id: response.data,
                Name: newContact.Name,
                Surname: newContact.Surname,
                Address: newContact.Address,
                Emails: emails,
                Telephones: telephones,
                Tags: tags
            })
            if (tempEmails != undefined) {
                tempEmails.forEach(function (mail) {
                    if (mail != "") {
                        var Em = { PersonId: response.data, Email1: mail }
                        var adding2 = myService.add_Email(Em);
                        adding2.then(function successCallback(response) {
                            console.log(response);
                        }, function errorCallback(response) {
                            return "Error";
                        });
                    }
                });
            }
            if (tempTels != undefined) {
                tempTels.forEach(function (tel) {
                    if (tel != "") {
                        var Tel = { PersonId: response.data, Telephone1: tel }
                        var adding2 = myService.add_Telephone(Tel);
                        adding2.then(function successCallback(response) {
                            console.log(response);
                        }, function errorCallback(response) {
                            return "Error";
                        });
                    }
                })
            }

            if (tempTags != undefined) {
                tempTags.forEach(function (tag) {
                    if (tag != "") {
                        var Tag = { PersonId: response.data, Tag1: tag }
                        var adding2 = myService.add_Tag(Tag);
                        adding2.then(function successCallback(response) {
                            console.log(response);
                        }, function errorCallback(response) {
                            return "Error";
                        });
                    }
                })
            }
            console.log(response);

        }, function errorCallback(response) {
            return "Error";
        });
        //na kraju, u brisemo ono sto je bilo u $scope.newContact kako se u unosu sljedeceg novog kontakta
        //ne bi pojavili podaci zadnje unesenog
        $scope.newContact = {};
    }
});