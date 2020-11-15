const Http = new XMLHttpRequest();
Http.open("GET", "https://projetinfo.alwaysdata.net/CatChaserAPI/scores");
Http.send();
haveData = false;
Http.onreadystatechange=(e)=>{
    if(Http.status == 200){
        if(!haveData){
            var data = Http.responseText;
            var jsonData = JSON.parse(data);
            console.log(jsonData);
            var body = document.getElementsByTagName("body")[0];
            var tbl = document.createElement("table");
                var tblBody = document.createElement("tbody");
                for (var i = 0; i < 20; i++) {
                var row = document.createElement("tr");
                for (var j = 0; j < 2; j++) {
                    var cell = document.createElement("td");
                    try{
                        if(j == 0){
                            var cellText = document.createTextNode(jsonData[i].name);
                        } else {
                            var cellText = document.createTextNode(jsonData[i].score);
                        }
                    } catch(error) {
                        var cellText = document.createTextNode("null");
                    }
                    cell.appendChild(cellText);
                    row.appendChild(cell);
                }
                tblBody.appendChild(row);
            }
            tbl.appendChild(tblBody);
            body.appendChild(tbl);
            tbl.setAttribute("border", "2");
            haveData = true;
        }
    } else {
        console.log("error : " + Http.status);
    }
}