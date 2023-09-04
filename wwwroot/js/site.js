// used git to clone it

// Home Page
const heroHeader = document.querySelector("#hero-header");
const wallPainting = document.querySelector(".wall");
const encausticPainting = document.querySelector(".encaustic");
const environmentPainting = document.querySelector(".environment");
const inkWashPainting = document.querySelector(".ink-wash");
const naturePainting = document.querySelector(".nature");
const spacePainting = document.querySelector(".space");
const waterColorPainting = document.querySelector(".water-color");

if (wallPainting) {
    wallPainting.addEventListener("mouseover", () => {
        heroHeader.setAttribute('class', '');
        heroHeader.classList.add("wall-hero-header");
    });
}

if (encausticPainting) {
    encausticPainting.addEventListener("mouseover", () => {
        heroHeader.setAttribute('class', '');
        heroHeader.classList.add("encaustic-hero-header");
    });
} 

if (environmentPainting) {
    environmentPainting.addEventListener("mouseover", () => {
        heroHeader.setAttribute('class', '');
        heroHeader.classList.add("environment-hero-header");
    });
} 

if (inkWashPainting) {
    inkWashPainting.addEventListener("mouseover", () => {
        heroHeader.setAttribute('class', '');
        heroHeader.classList.add("ink-wash-hero-header");
    });
} 

if (naturePainting) {
    naturePainting.addEventListener("mouseover", () => {
        heroHeader.setAttribute('class', '');
        heroHeader.classList.add("nature-hero-header");
    });
} 

if (spacePainting) {
    spacePainting.addEventListener("mouseover", () => {
        heroHeader.setAttribute('class', '');
        heroHeader.classList.add("space-hero-header");
    });
}

if (waterColorPainting) {
    waterColorPainting.addEventListener("mouseover", () => {
        heroHeader.setAttribute('class', '');
        heroHeader.classList.add("water-color-hero-header");
    });
}


// Home Corosol
const gallery = document.querySelector("#home-corosol-gallery");

// declaring other variables
// we need to add them here instead of adding them at the top because they will be null
// as the html elements would not have been created there
let activeIndex = 0;
const leftButton = document.querySelectorAll(".left-button");
const rightButton = document.querySelectorAll(".right-button");
const figures = document.querySelectorAll(".figure");
const allImagesSection = document.querySelector(".allImages");

// now since we have the ul li img elements in the DOM, we will be able to select them
let images = document.querySelectorAll(".allImages > ul > li > img");

// creating a function to add css class "gray" to all the images in the ul li and removing
// the css class from a single image which is actively displayed on the screen 
function addGrayClass(arr, index) {
  arr.forEach((element) => {
    element.classList.add("gray");
  });

  arr[index].classList.remove("gray");
}

// calling the function to take initial effect of adding gray class to all images except
// the first one
if (images && images.length > 0) addGrayClass(images, 0);

// ul li images
images.forEach((ele, index) => { // index => nextIndex
  ele.addEventListener("click", () => {
    if (index !== activeIndex) { // on clicking an image, obviously the index will be different if not clicking on the same image
      // selecting the current figure using the data-index="..."; and selecting the
      // next figure using the clicked image's index data-index="...";
      const currentFiguer = document.querySelector(`[data-index="${activeIndex}"]`);
      nextFigure = document.querySelector(`[data-index="${index}"]`);
      // see Notes below for more information about data-status="..." values

      // changing (adding) data-status="after/before" and data-status="becoming-active-from-before/becoming-active-from-after"
      // respective (according) to the next index (clicked index)
      currentFiguer.dataset.status = index > activeIndex ? "after" : "before";
      nextFigure.dataset.status = index > activeIndex ? "becoming-active-from-before" : "becoming-active-from-after";

      // setting a very short delay before we add the data-status="active" for the smooth effect
      setTimeout(() => {
        nextFigure.dataset.status = "active";
        // changing the next index to the active index so that current index do not keep
        // pointing to the same index each and every time
        activeIndex = index;
      });

      // adding the gray class to all the ul li images except new current image (next index / clicked index) 
      addGrayClass(images, index);
    }
  });
});

// for all the left button we are adding the event listener to change the image using
// setActiveFigure function
leftButton.forEach((ele) => {
  ele.addEventListener("click", () => {
    // the math logic explained in the Notes below
    const nextIndex = (activeIndex - 1 + figures.length) % figures.length;
    // passing the next index to the function
    setActiveFigure(nextIndex);
  });
});

// for all the right button we are adding the event listener to change the image using
// setActiveFigure function
rightButton.forEach((ele) => {
  ele.addEventListener("click", () => {
    // the math logic explained in the Notes below
    const nextIndex = (activeIndex + 1) % figures.length;
    // passing the next index to the function
    setActiveFigure(nextIndex);
  });
});

// this function utilizes the next index from the buttons click events and toggling the
// data-status values accordingly
function setActiveFigure(nextIndex) {
  // selecting the current figure using the data-index="..."; and selecting the
  // next figure using the next index parameter index data-index="...";
  const currentFiguer = document.querySelector(`[data-index="${activeIndex}"]`);
        nextFigure = document.querySelector(`[data-index="${nextIndex}"]`);

  // checking if the next index is greater then the active index and adding the data-status
  // active/before values respectively to the current active figure
  currentFiguer.dataset.status = nextIndex > activeIndex ? "after" : "before";
  // then adding the data-status becoming-active-from-before/becoming-active-from-after
  // accordingly
  nextFigure.dataset.status = nextIndex > activeIndex ? "becoming-active-from-before" : "becoming-active-from-after";

  // setting a very little delay before adding data-status="active" for the smooth effect
  setTimeout(() => {
    nextFigure.dataset.status = "active";
    // changing the next index to the active index so that current index do not keep
    // pointing to the same index each and every time
    activeIndex = nextIndex;
  });

  // adding the gray class to all the ul li images except new current image (next index) 
  addGrayClass(images, nextIndex);
}








// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// for masonry
var colc = new Colcade(".grid", {
    columns: ".grid-col",
    items: ".grid-item",
});