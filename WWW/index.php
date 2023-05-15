<?php
    session_start();
	if((isset($_SESSION['logged']))&&($_SESSION['logged']==true))
	{
		header('Location: main.php');
		exit();
	}
    //background-color: rgba(55, 55, 55, 0.7);
?>
<!DOCTYPE html>
<html lang="pl-PL">
<head>
    <?php
        require_once "settings.php";
        echo $head;
    ?>
    <link rel='stylesheet' href='css/Index.css' type='text/css' />
</head>
<body>
    <script src="js/jquery.js"></script>
    <div id="zegar"></div>
    <main>
        <section id="reg"> 
            <!--<button onclick="test()">GET</button>-->
            <!--<form >-->
            <input type="text" id="nick" placeholder="NICK"> <br>
            <input type="text" id="email" placeholder="E-MAIL"><br>
            <input type="password" id="pass1" placeholder="HASŁO"><br>
            <input type="password" id="pass2" placeholder="POWTÓRZ HASŁO"><br>
            <label><input type="checkbox" id="regulamin"/> Akceptuję regulamin</label><br> <br>
            <!--<div class="g-recaptcha" data-sitekey="6LcN910cAAAAANRvlAAjgz0sroy6tG2gXUuh8GGu"></div><br>-->
            <button onclick="register()">Zarejestruj się</button>
            <div id="registerError" style="color: #FF0000;"></div>
            <!--</form>-->
        </section>
        <section id="login">
            <form action="loginScript.php" method="post">
                <input type="text" name="login" id="nameLogin" placeholder="NICK"> <br>
                <input type="password" name="pass" id="passLogin" placeholder="HASŁO"> <br> <br>
                <button>Zaloguj się</button>
                <!--<button onclick="login()">Zaloguj się</button>-->
            </form>
            <div id="loginError"><?php if(isset($_SESSION['logERROR'])) echo $_SESSION['logERROR']; ?></div>
        </section>
        
        <div id="content"></div>
    </main>
    <div><img src="img/background.jpg" alt="" style="width: 100%; height: 100%; position: absolute; left: 0; top: 0; z-index: -1" /></div>
</body>
</html>