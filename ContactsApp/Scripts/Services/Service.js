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

    this.create_DB = function () {
        return $http.get('/Home/fillDB');
    }

    this.get_Contacts = function () {
        return $http.get('/Home/getAll')
    }

    this.delete_Contact = function (ConId) {
        return $http({
            method : 'post',
            url : '/Home/deleteContact',
            params : {
                ConId : JSON.stringify(ConId)
            }
        });

    }

    this.update_Contact = function (Con) {
        return $http({
            method : 'post',
            url : '/Home/updateContact',
            data: JSON.stringify(Con),
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
            method : 'post',
            url : '/Home/addContact',
            data: JSON.stringify(Con),
            dataType: "json"
        });
    }
});