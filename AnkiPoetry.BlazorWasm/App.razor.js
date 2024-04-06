export function downloadContent(element_id, file_name) {
    var input_element = document.getElementById(element_id);
    var text = input_element.value;

    var el = document.createElement("a");
    el.href = "data:text/plain;charset=UTF-8," + encodeURIComponent(text);
    el.setAttribute("download", file_name);
    el.click();
}
