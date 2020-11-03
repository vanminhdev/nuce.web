function onChangeCertificationSelect() {
  const selectVal = document.getElementById("certificationSelect").value;
  const textAreaEle = document.getElementById("reasonConfirm");

  if (parseInt(selectVal) === 10) {
    textAreaEle.style.display = "block";
  } else {
    textAreaEle.style.display = "none";
  }
}

function handleClickDropdown() {
  const dropdownEle = document.querySelector("#dropdown-icon");
  const settingEle = document.getElementById("setting-wrp");

  if (!dropdownEle.classList.contains("rotate-90")) {
    dropdownEle.classList.add("rotate-90");
    settingEle.style.display = "block";
  } else {
    dropdownEle.classList.remove("rotate-90");
    settingEle.style.display = "none";
  }
}

$("#datePicker input").datepicker({
  language: "vi",
  autoclose: true,
  todayHighlight: true,
  format: "dd/mm/yyyy",
});

document
  .querySelector(".custom-file-input")
  .addEventListener("change", function (e) {
    var fileName = document.getElementById("input-file").files[0].name;
    var nextSibling = e.target.nextElementSibling;
    nextSibling.innerText = fileName;
  });
