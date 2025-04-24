/// scrollToTop
export function scrollToTop(containerId) {
    const container = document.getElementById(containerId);
    if (container) {
        container.scrollTop = 0;
    }
}

/// getTextBoxValue
export function getTextBoxValue(textBoxId) {
    const textBox = document.getElementById(textBoxId);
    return (textBox) ? textBox.value : null;
}

/// setFocus
export function setFocus(componentId) {
    var component = document.getElementById(componentId);
    if (component) {
        component.focus();
    }
}

/// showAlert
export function showAlert(message) {
    alert(message);
}