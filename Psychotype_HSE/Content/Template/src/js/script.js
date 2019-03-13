var coll = document.getElementsByClassName("hidebutton");
var inpBut = document.getElementsByClassName("searchbutton")[0];
var inpLine = document.getElementById("query")
var i;

document.addEventListener("DOMContentLoaded", function () {
    inpBut.disabled = false;
    inpLine.disabled = false;
});

inpBut.addEventListener("mouseup", function () {
    inpBut.disabled = true;
    inpLine.disabled = true;
    if (inpLine.value === "undefined") {
        window.location.replace('/?query=');
    }
    else window.location.replace('/?query=' + inpLine.value);
});

inpLine.addEventListener("keydown", function (e) {
    if (e.key === "Enter")
        inpBut.dispatchEvent(new Event('mouseup'));//.createEvent("mouseup");
});

for (i = 0; i < coll.length; i++) {
  coll[i].addEventListener("click", function() {
    this.classList.toggle("active");
    var img = this.childNodes.item(1);
    var content = this.parentElement.nextElementSibling;
    if (content.style.display === "block") {
        content.style.display = "none";
        img.setAttribute('style', 'transform:rotate(180deg)');
      
    } else {
        content.style.display = "block";
        img.setAttribute('style', 'transform:rotate(0deg)');
    }
  });
}
