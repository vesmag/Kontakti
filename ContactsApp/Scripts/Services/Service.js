var myService = function ($http) {
    this.databaseFilled = false;

    this.create_DB = function () {
        if (this.databaseFilled === false) {
            this.databaseFilled = true;
            return $http.get('/Home/fillDB');
        }           
        else
            return "Database already filled.";
    }

    this.get_Contacts = function () {
        return $http.get('/Home/getAll')
    }

    this.delete_Contact = function (ConId) {
        console.log(ConId);
        return $http({
            method : 'post',
            url : '/Home/deleteContact',
            params : {
                ConId : JSON.stringify(ConId)
            }
        });

    }

    this.update_Contact = function (Con) {
        console.log(Con);
        return $http({
            method : 'post',
            url : '/Home/updateContact',
            data: JSON.stringify(Con),
            dataType: "json"
        })
    }

    this.name_Search = function (searchString) {
        console.log(searchString);
        return $http({
            method: 'get',
            url: '/Home/searchByName',
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
    }

    this.add_Contact = function (Con) {
        return $http({
            method : 'post',
            url : '/Home/addContact',
            data: JSON.stringify(Con),
            dataType: "json"
        });
    }
}

myService.$inject = ['$http'];