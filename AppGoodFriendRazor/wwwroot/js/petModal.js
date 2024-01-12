function openPetEditModal(petId = '', petName = '', petKind = '', petMood = '') {
    var petForm = document.getElementById('editPetForm');

    // Set the form action for updating an existing pet
    petForm.action = '/FriendDetails?handler=UpdatePet'; // Adjust this URL as needed

    // Set the form fields
    document.getElementById('petId').value = petId;
    document.getElementById('petName').value = petName;
    document.getElementById('petKind').value = petKind;
    document.getElementById('petMood').value = petMood;

    // Open the modal
    var petModal = new bootstrap.Modal(document.getElementById('editPetModal'));
    petModal.show();
}

function openAddPetModal() {
    var addPetForm = document.getElementById('addPetForm');
    addPetForm.reset(); // Resets the form fields to default values
    var addPetModal = new bootstrap.Modal(document.getElementById('addPetModal'));
    addPetModal.show();
}
