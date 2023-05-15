<img src='' width='150' height='150' alt='' id='previmg'>
<div id='info'>
    <div>
        Długość produkcji: 
        <div id='prodDate'>

        </div>
    </div>
    <div>
        Wymagania do rozbudowy na poziom 
        <div id='prodLVL'></div> <br>
        <div class='infoimg'>
            <img src='img/metal.png' alt='' width='40' height='40'> <br>
            <div id='metalInfo'></div>
        </div>
        <div class='infoimg'>
            <img src='img/crystal.png' alt='' width='40' height='40'> <br>
            <div id='crystalInfo'></div>
        </div>
        <button onclick="upgradeBuilding(<?php echo $_GET['planet'] ?>,'<?php echo $_GET['building'] ?>')">UPGRADE</button>
    </div>
</div>
<div id='desc'>Lorem ipsum dolor sit amet consectetur adipisicing elit.</div>