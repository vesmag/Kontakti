ContactsApp.service("myService", function ($http) {
    //funkcija svih metoda u serviceu je pozivanje odgovarajuce funkcije u HomeControlleru preko $http poziva
    this.get_Contact_By_Id = function (ConId) {
        return $http({
            method: 'get',
            url: '/Home/getContactById',
            params: {
                ConId: ConId
            }
        });

    }

    this.get_Contacts = function () {
        return $http.get('/Home/getAll')
    }
    
    this.delete_Contact = function (ConId) {
        return $http({
            method: 'post',
            url: '/Home/deleteContact',
            params: {
                ConId: ConId
            }
        });
    }

    this.delete_Email = function (Em) {
        return $http({
            method: 'post',
            url: '/Home/deleteEmail',
            data: JSON.stringify(Em),
            dataType: "json"
        })
    }

    this.delete_Telephone = function (Tel) {
        return $http({
            method: 'post',
            url: '/Home/deleteTelephone',
            data: JSON.stringify(Tel),
            dataType: "json"
        })
    }

    this.delete_Tag = function (Tag) {
        return $http({
            method: 'post',
            url: '/Home/deleteTag',
            data: JSON.stringify(Tag),
            dataType: "json"
        })
    }

    this.update_Name = function (Id, Name) {
        return $http({
            method: 'get',
            url: '/Home/updateName',
            params: {
                Id: Id,
                Name: Name
            }
        });
    }

    this.update_Surname = function (Id, Surname) {
        return $http({
            method: 'get',
            url: '/Home/updateSurname',
            params: {
                Id: Id,
                Surname: Surname
            }
        });
    }

    this.update_Address = function (Id, Address) {
        return $http({
            method: 'get',
            url: '/Home/updateAddress',
            params: {
                Id: Id,
                Address: Address
            }
        });
    }


    this.update_Email = function (Em) {
        return $http({
            method: 'post',
            url: '/Home/updateEmail',
            data: JSON.stringify(Em),
            dataType: "json"
        })
    }

    this.update_Telephone = function (Tel) {
        return $http({
            method: 'post',
            url: '/Home/updateTelephone',
            data: JSON.stringify(Tel),
            dataType: "json"
        })
    }

    this.update_Tag = function (Tag) {
        return $http({
            method: 'post',
            url: '/Home/updateTag',
            data: JSON.stringify(Tag),
            dataType: "json"
        })
    }

    this.search = function (searchString) {
        return $http({
            method: 'get',
            url: '/Home/search',
            params: {
                searchString: searchString
            }
        });
    }

    this.add_Contact = function (Con) {
        return $http({
            method: 'post',
            url: '/Home/addContact',
            data: JSON.stringify(Con),
            dataType: "json"
        });
    }

    this.add_Email = function (Em) {
        return $http({
            method: 'post',
            url: '/Home/addEmail',
            data: JSON.stringify(Em),
            dataType: "json"
        });
    }

    this.add_Telephone = function (Tel) {
        return $http({
            method: 'post',
            url: '/Home/addTelephone',
            data: JSON.stringify(Tel),
            dataType: "json"
        });
    }

    this.add_Tag = function (Tag) {
        return $http({
            method: 'post',
            url: '/Home/addTag',
            data: JSON.stringify(Tag),
            dataType: "json"
        });
    }
});