var myService = function ($http) {
    this.get_Contacts = function () {
        debugger;
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
            params: {
                ConId : Con.Id,
                ConName: Con.Name,
                ConSurname: Con.Surname,
                ConAddress: Con.Address,
                ConEmail: Con.Email,
                ConTelephone: Con.Telephone,
                ConTags: Con.Tags
            }
        })
    }
}

myService.$inject = ['$http'];