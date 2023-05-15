//<---------------------------------------------------------ZMIENNE_GLOBALNE------------------------------------------------>
var XHR=null;

//<---------------------------------------------------------------AJAX------------------------------------------------------------>
function ajaxInit()
{

	try
	{
		XHR = new XMLHttpRequest();
	}
	catch(e)
	{
		try
		{
			XHR = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch(e2)
		{
			try
			{
				XHR = new ActiveXObject("Microsoft.XMLHTTP");
			}
			catch(e3)
			{
				alert("Twoja przeglądarka nie obsługuje AJAX");
			}
		}
	}
	return XHR;
}

function test()
{
	XHR = ajaxInit();
	if(XHR == null) return;
	XHR.open("GET","https://localhost:8888/WeatherForecast/dupa",true);
	XHR.onreadystatechange = function()
	{
		if(XHR.readyState == 4)
		{
			//document.getElementById("content").innerHTML = XHR.responseText;
			document.getElementById("content").innerHTML = JSON.stringify({nick: document.getElementById("nick").value, email: document.getElementById("email").value, pass1: document.getElementById("pass1").value, 
			pass2: document.getElementById("pass2").value, regul: document.getElementById("regulamin").checked});
		}
	}
	XHR.send();
}

function register()
{
	XHR = ajaxInit();
	console.log("response");
	if(XHR == null) return;
	
	toSend = 
	{
		nick: document.getElementById("nick").value,
		email: document.getElementById("email").value,
		pass1: document.getElementById("pass1").value, 
		pass2: document.getElementById("pass2").value,
		regul: document.getElementById("regulamin").checked
	};
	jsonString = JSON.stringify(toSend);

	XHR.open("POST","https://localhost:8888/register",true);
	XHR.setRequestHeader("Content-Type", "application/json");
	XHR.onreadystatechange = function()
	{
		//alert(XHR.responseText);
		if(XHR.readyState == 4 && XHR.status != 200)
		{
			document.getElementById("registerError").innerHTML = XHR.responseText;
		}
		else if (XHR.readyState == 4 && XHR.status == 200)
		{
			document.getElementById("nick").value = "";
			document.getElementById("email").value = "";
			document.getElementById("pass1").value = "";
			document.getElementById("pass2").value = "";
			document.getElementById("regulamin").checked = false;
		}
	};
	XHR.send(jsonString);
}

function refreshResource(planet)
{
	XHR = ajaxInit();
	if(XHR == null) return;

	XHR.open("PATCH","https://localhost:8888/resources?planet="+planet,true);
	XHR.onreadystatechange = function()
	{
		if (XHR.readyState == 4 && XHR.status == 200)
		{
			var json = XHR.responseText;
			var obj = JSON.parse(json);

			document.getElementById("Metal").innerHTML = Math.floor(obj.metal);
			document.getElementById("Crystal").innerHTML = Math.floor(obj.crystal);
			document.getElementById("Deuter").innerHTML = Math.floor(obj.deuter);
			if (obj.construction != "")
			{
				document.getElementById("queueIMG").src = "img/"+obj.construction+".jpg" ;
				var SEK = obj.construction_sek;
				var weeks = Math.floor(SEK / 604800);
				var days = Math.floor((SEK - (weeks * 604800)) / 86400);
				var hours = Math.floor((SEK - (weeks * 604800) - (days * 86400)) / 3600);
				var minutes = Math.floor((SEK - (weeks * 604800) - (days * 86400) - (hours * 3600)) / 60);
				var seconds = Math.floor((SEK - (weeks * 604800) - (days * 86400) - (hours * 3600) - (minutes * 60)));

				document.getElementById("queueBuilding").innerHTML = weeks + " tyg " + days + " d " + hours + " h " + minutes + " m " + seconds + " s";
				//document.getElementById("queueBuilding").innerHTML = obj.construction_sek;
			}
			
			if (obj.shipyard != "")
			{
				document.getElementById("queueShipyardIMG").src = "img/"+obj.shipyard+".jpg" ;
				var SEK = obj.shipyard_sek;
				var weeks = Math.floor(SEK / 604800);
				var days = Math.floor((SEK - (weeks * 604800)) / 86400);
				var hours = Math.floor((SEK - (weeks * 604800) - (days * 86400)) / 3600);
				var minutes = Math.floor((SEK - (weeks * 604800) - (days * 86400) - (hours * 3600)) / 60);
				var seconds = Math.floor((SEK - (weeks * 604800) - (days * 86400) - (hours * 3600) - (minutes * 60)));

				document.getElementById("queueShipyard").innerHTML = weeks + " tyg " + days + " d " + hours + " h " + minutes + " m " + seconds + " s";
				//document.getElementById("queueBuilding").innerHTML = obj.construction_sek;
			}

			//document.getElementById("Metal").innerHTML = obj.metal;
			//document.getElementById("Crystal").innerHTML = obj.crystal;
			//document.getElementById("Deuter").innerHTML = obj.deuter;
		}
		else
		{
			//console.log(XHR.responseText);
		}
	};
	XHR.send();
	setTimeout("refreshResource("+planet+")",1000);
}

function showBuilding(planet, building)
{

	XHR = ajaxInit();
	if(XHR == null) return;

	XHR.open("GET","https://localhost:8888/buildings/" + building + "?planet="+planet,true);
	XHR.onreadystatechange = function()
	{
		
		if (XHR.readyState == 4 && XHR.status == 200)
		{
			var json = XHR.responseText;
			var obj = JSON.parse(json);

			document.getElementById("metalInfo").innerHTML = Math.floor(obj.metal);
			document.getElementById("crystalInfo").innerHTML = Math.floor(obj.crystal);
			document.getElementById("prodDate").innerHTML = Math.floor(obj.sek);
			document.getElementById("prodLVL").innerHTML = Math.floor(obj.lvl);
			document.getElementById("previmg").src = "img/"+building+".jpg" ;

			var SEK = Math.floor(obj.sek);
			var weeks = Math.floor(SEK / 604800);
			var days = Math.floor((SEK - (weeks * 604800)) / 86400);
			var hours = Math.floor((SEK - (weeks * 604800) - (days * 86400)) / 3600);
			var minutes = Math.floor((SEK - (weeks * 604800) - (days * 86400) - (hours * 3600)) / 60);
			var seconds = Math.floor((SEK - (weeks * 604800) - (days * 86400) - (hours * 3600) - (minutes * 60)));

			document.getElementById("prodDate").innerHTML = weeks + " tyg " + days + " d " + hours + " h " + minutes + " m " + seconds + " s";
		}
		else
		{
			//console.log(XHR.responseText);
		}
	};
	XHR.send();
}

function upgradeBuilding(planet, building)
{
	XHR = ajaxInit();
	if(XHR == null) return;

	XHR.open("PATCH","https://localhost:8888/buildings/" + building + "?planet="+planet,true);
	XHR.onreadystatechange = function()
	{
		if (XHR.readyState == 4 && XHR.status == 200)
		{
			var json = XHR.responseText;
			var obj = JSON.parse(json);

			console.log(XHR.responseText);
		}
		else
		{
			console.log(XHR.responseText);
		}
	};
	XHR.send();
}

function showBuildingPanel(planet, building)
{
	XHR = ajaxInit();
	if(XHR != null)
	{
		//XHR.open("POST","buildingScreen.php",true);
		XHR.open("GET","buildingScreen.php?planet="+planet+"&building="+building,true);
		XHR.onreadystatechange = function()
		{
			if(XHR.readyState == 4)
			{
				document.getElementById("preview").innerHTML = XHR.responseText;
			}
		}
		XHR.send();
	}

	setTimeout("showBuilding("+planet+", '"+building+"')",0);
}

function showShipyardPanel(planet, building)
{
	XHR = ajaxInit();
	if(XHR != null)
	{
		//XHR.open("POST","buildingScreen.php",true);
		XHR.open("GET","shipyardScreen.php?planet="+planet+"&building="+building,true);
		XHR.onreadystatechange = function()
		{
			if(XHR.readyState == 4)
			{
				document.getElementById("preview").innerHTML = XHR.responseText;
			}
		}
		XHR.send();
	}

	setTimeout("showShip("+planet+", '"+building+"')",0);
}

function upgradeShip(planet, building)
{
	XHR = ajaxInit();
	if(XHR == null) return;

	XHR.open("PATCH","https://localhost:8888/fleet/" + planet + "/" + building,true);
	XHR.onreadystatechange = function()
	{
		if (XHR.readyState == 4 && XHR.status == 200)
		{
			var json = XHR.responseText;
			var obj = JSON.parse(json);

			console.log(XHR.responseText);
		}
		else
		{
			console.log(XHR.responseText);
		}
	};
	XHR.send();
}

function showShip(planet, building)
{

	XHR = ajaxInit();
	if(XHR == null) return;

	XHR.open("GET","https://localhost:8888/fleet/"+planet+"/"+building,true);
	XHR.onreadystatechange = function()
	{
		
		if (XHR.readyState == 4 && XHR.status == 200)
		{
			var json = XHR.responseText;
			var obj = JSON.parse(json);

			document.getElementById("metalInfo").innerHTML = Math.floor(obj.metal);
			document.getElementById("crystalInfo").innerHTML = Math.floor(obj.crystal);
			document.getElementById("prodDate").innerHTML = Math.floor(obj.sek);
			//document.getElementById("prodLVL").innerHTML = Math.floor(obj.lvl);
			document.getElementById("previmg").src = "img/"+building+".jpg" ;

			var SEK = Math.floor(obj.sek);
			var weeks = Math.floor(SEK / 604800);
			var days = Math.floor((SEK - (weeks * 604800)) / 86400);
			var hours = Math.floor((SEK - (weeks * 604800) - (days * 86400)) / 3600);
			var minutes = Math.floor((SEK - (weeks * 604800) - (days * 86400) - (hours * 3600)) / 60);
			var seconds = Math.floor((SEK - (weeks * 604800) - (days * 86400) - (hours * 3600) - (minutes * 60)));

			document.getElementById("prodDate").innerHTML = weeks + " tyg " + days + " d " + hours + " h " + minutes + " m " + seconds + " s";
		}
		else
		{
			//console.log(XHR.responseText);
		}
	};
	XHR.send();
}

function showGalaxy()
{
	XHR = ajaxInit();
	if(XHR != null)
	{
		//XHR.open("POST","buildingScreen.php",true);
		XHR.open("GET","galaxyScreen.php",true);
		XHR.onreadystatechange = function()
		{
			if(XHR.readyState == 4)
			{
				document.getElementById("preview").innerHTML = XHR.responseText;
			}
		}
		XHR.send();
	}

	setTimeout("generateGalaxy()",10);
}

function generateGalaxy()
{
	var system = document.getElementById("system").value;
	document.getElementById("galaxyContent").innerHTML = "";
	XHR = ajaxInit();
	if(XHR != null)
	{
		XHR.open("GET","https://localhost:8888/galaxy/" + system ,true);
		XHR.onreadystatechange = function()
		{
			if(XHR.readyState == 4)
			{
				//alert(XHR.responseText);
				var json = XHR.responseText;
				var obj = JSON.parse(json);
				//alert(obj[1].playerID);
				for (let i = 0; i < 15; i++) 
				{
					document.getElementById("galaxyContent").innerHTML += 
					"<div class=\"planetInfo\"> " 
					+ " <img src=\"img/planeta.png\"  width=\"30\" height=\"30\" class=\"planetInfoItem\"> <div id=\"planetName\" class=\"planetInfoItem\">" 
					+ obj[i].name + "</div> <div id=\"ownerName\" class=\"planetInfoItem\">" + obj[i].playerName + "</div> <input type=\"button\" value=\"flota\" class=\"planetInfoItem\">  </div>";
				}
				
			}
		}
		XHR.send();
	
	}
}