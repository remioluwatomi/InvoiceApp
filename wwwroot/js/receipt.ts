const DELETE_INVOICE_FORM = document.getElementById(
  "delete-invoice-form"
) as HTMLFormElement;

const MARK_AS_PAID_FORM = document.getElementById(
  "mark-as-paid-form"
) as HTMLFormElement;

const MARK_AS_PAID_BTNS = [...document.getElementsByClassName("mark-as-paid ")];
const DELETE_BTNS = [...document.getElementsByClassName("delete")];

MARK_AS_PAID_BTNS.forEach((btn) => {
  (btn as HTMLButtonElement).addEventListener("click", markInvoiceAsPaid);
});

DELETE_BTNS.forEach((btn) => {
  (btn as HTMLButtonElement).addEventListener("click", deleteInvoice);
});

function markInvoiceAsPaid(e: MouseEvent) {
  MARK_AS_PAID_FORM?.submit();
}

function deleteInvoice(e: MouseEvent) {
  DELETE_INVOICE_FORM?.submit();
}
