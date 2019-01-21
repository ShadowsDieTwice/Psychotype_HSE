var coll = document.getElementsByClassName("hidebutton");
var i;

/*
for (i = 0; i < coll.length; i++) {
  coll[i].addEventListener("click", function() {
    this.classList.toggle("active");

    var content = this.parentElement;
    
    if (content.style.display === "block") {
      content.style.display = "none";
    } else {
      content.style.display = "block";
    }
  });
}
*/

for (i = 0; i < coll.length; i++) {
  coll[i].addEventListener("click", function() {
    this.classList.toggle("active");
    var img = this.childNodes.item(1);
    var content = this.parentElement.nextElementSibling;
    if (content.style.display === "block") {
      content.style.display = "none";
      img.setAttribute('style','transform:rotate(0deg)');
    } else {
      content.style.display = "block";
      img.setAttribute('style','transform:rotate(180deg)');
    }
  });
}
