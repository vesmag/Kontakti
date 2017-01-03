ContactsApp.service("myService", function ($http) {

    this.get_Contact_By_Id = function (ConId) {
        return $http({
            method: 'get',
            url: '/Home/getContactById',
            params: {
                ConId: ConId
            }
        });

    }
    /*

    this.create_DB = function () {
        if (this.databaseFilled === false) {
            this.databaseFilled = true;
            return $http.get('/Home/fillDB');
        }
        else
            return "Database already filled.";
    }*/

    this.get_Contacts = function () {
        return $http.get('/Home/getAll')
    }
    
    this.delete_Contact = function (ConId) {
        console.log(ConId);
        return $http({
            method: 'post',
            url: '/Home/deleteContact',
            params: {
                ConId: JSON.stringify(ConId)
            }
        });
    }
    /*
    this.update_Contact = function (Con) {
        console.log(Con);
        return $http({
            method: 'post',
            url: '/Home/updateContact',
            data: JSON.stringify(Con),
            dataType: "json"
        })
    }

    this.search = function (searchString) {
        console.log(searchString);
        return $http({
            method: 'get',
            url: '/Home/search',
            params: {
                searchString: searchString
            }
        });
    }

    this.surname_Search = function (searchString) {
        console.log(searchString);
        return $http({
            method: 'get',
            url: '/Home/searchBySurname',
            params: {
                searchString: searchString
            }
        });
    }

    this.tag_Search = function (searchString) {
        console.log(searchString);
        return $http({
            method: 'get',
            url: '/Home/searchByTag',
            params: {
                searchString: searchString
            }
        });
    }*/

    this.add_Contact = function (Con) {
        console.log("u serviceu");
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