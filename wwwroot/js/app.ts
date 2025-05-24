const getFormTemplateString: (i: number) => string = (i) => {
  return `
                          <fieldset class="item-fieldset" data-itemIndex="${i}">
                              
                              <div class='form-group' data-name='Name'>
                                  <label for="Items_${i}__Name">Item Name</label>
                                  <input type="text" data-val="true" data-val-required="Item Name is required" id="Items_${i}__Name" name="Items[${i}].Name" />

                                  <span class="text-danger field-validation-valid" data-valmsg-for="Items[${i}].Name" data-valmsg-replace="true"></span>
                      
                              </div>
  
                              <div class="mini-form-wrapper">
                                <div class='form-group' data-name='Quantity'>
                                  <label for="Items_${i}__Quantity">Qty</label>
                                  
                                  <input type="number" data-val="true" data-val-required="Quantity is required" id="Items_${i}__Quantity" name="Items[${i}].Quantity"/>
                                  
                                  <span class="text-danger field-validation-valid" data-valmsg-for="Items[${i}].Quantity" data-valmsg-replace="true"></span>
                                </div>
  
                              <div class='form-group' data-name='Price'>
                                  <label for="Items_${i}__Price">Price</label>
                                  <input type="text" data-val="true" data-val-required="Price is required" id="Items_${i}__Price" name="Items[${i}].Price" />

                                  <span class="text-danger field-validation-valid" data-valmsg-for="Items[${i}].Price" data-valmsg-replace="true"></span>
                              </div>
  
                              <div class='form-group readonly-form-group'>
                                  <label>Total</label>
                                  <input readonly/>
                              </div>
  
                              <button class="delete-item-wrapper" type="button" data-itemIndex="${i}">
                              <img alt="delete item" src="/assets/icon-delete.svg" /></button>
                              </div>
                      </fieldset>
  
  `;
};

const ASIDE_FORM_BOX = document.querySelector(".form-box");
const INVOICE_BOX = document.querySelector(".invoice-box");

const OPEN_INVOICE_FORM = () => {
  INVOICE_BOX?.classList.add("form-box-opened");
  ASIDE_FORM_BOX?.classList.add("show");
};

const ADD_NEW_INVOICE_BTN = document.querySelector<HTMLButtonElement>(
  ".new-invoice-btn-wrapper button"
);

const EDIT_ADD_NEW_INVOICE_BTNS = [
  ...document.querySelectorAll<HTMLButtonElement>("button.edit.activity-btn"),
];

const SHOW_INVOICE_FORM_BTNS = [
  ...(ADD_NEW_INVOICE_BTN ? [ADD_NEW_INVOICE_BTN] : []),
  ...EDIT_ADD_NEW_INVOICE_BTNS,
];

SHOW_INVOICE_FORM_BTNS.forEach((btn) => {
  btn.addEventListener("click", OPEN_INVOICE_FORM);
});

const DISCARD_FORM_BTN = document.querySelector(".discard-btn");

DISCARD_FORM_BTN?.addEventListener("click", () => {
  ASIDE_FORM_BOX?.classList.remove("show");
  setTimeout(() => INVOICE_BOX?.classList.remove("form-box-opened"), 300);
});

//add new item interactivity

const ADD_NEW_ITEM_BTN =
  document.querySelector<HTMLButtonElement>(".new-item-btn");

const itemsWrapper = document.querySelector<HTMLDivElement>(
  ".bill-items-wrapper"
);

const UPDATE_INVOICE_INDEX = () => {
  let itemFieldsets = [
    ...(itemsWrapper as HTMLDivElement).getElementsByTagName("fieldset"),
  ];

  itemFieldsets.forEach((f, i) => {
    f.setAttribute("data-itemIndex", `${i}`);

    let formGroups = [...f.getElementsByClassName("form-group")];

    for (let j = 0; j < formGroups.length; j++) {
      let formGroup = formGroups[j];
      if (formGroup.classList.contains("readonly-form-group")) continue;

      let dataName = formGroup.getAttribute("data-name");
      let groupLabel, groupSpan, groupInput;

      groupLabel = formGroup.getElementsByTagName(
        "label"
      )[0] as HTMLLabelElement;

      groupSpan = formGroup.getElementsByTagName("span")[0] as HTMLSpanElement;
      groupInput = formGroup.getElementsByTagName(
        "input"
      )[0] as HTMLInputElement;

      let indexString = `Items[${i}].${dataName}`;
      let underScoreString = `Items_${i}__${dataName}`;

      groupLabel.setAttribute("for", underScoreString);

      groupInput.setAttribute("id", underScoreString);
      groupInput.setAttribute("name", indexString);

      groupSpan.setAttribute("for", indexString);
      groupSpan.setAttribute("data-valmsg-for", indexString);
    }

    let fieldsetDelBtn = f.getElementsByClassName(
      "delete-item-wrapper"
    )[0] as HTMLButtonElement;
    fieldsetDelBtn.setAttribute("data-itemIndex", `${i}`);
  });
};

const ADD_NEW_ITEM_DELEGATE = () => {
  let items = document.querySelectorAll("fieldset.item-fieldset");
  let itemsLen = items.length;

  if (itemsWrapper) itemsWrapper.innerHTML += getFormTemplateString(itemsLen);
};

ADD_NEW_ITEM_BTN?.addEventListener("click", ADD_NEW_ITEM_DELEGATE);

//remove item interactivity
if (itemsWrapper) {
  itemsWrapper.addEventListener("click", (evt) => {
    let isRemoveBtn = false;
    let removeBtn = (evt.target as HTMLElement).closest("button");

    if (removeBtn && removeBtn.classList.contains("delete-item-wrapper"))
      isRemoveBtn = true;

    if (!isRemoveBtn) return;
    const elIndex = removeBtn?.dataset.itemindex;

    if (!elIndex) return;

    const fieldSetToRemove = itemsWrapper.querySelector<HTMLFieldSetElement>(
      `fieldset[data-itemindex="${elIndex}"]`
    );

    if (fieldSetToRemove) fieldSetToRemove.remove();
    UPDATE_INVOICE_INDEX();
  });
}

const INVOICE_FORM = document.getElementById("invoice-form");

INVOICE_FORM?.addEventListener("submit", (e) => {
  e.preventDefault();
  // UPDATE_INVOICE_INDEX();
  const formEl = e.target as HTMLFormElement;

  formEl.submit();
});
