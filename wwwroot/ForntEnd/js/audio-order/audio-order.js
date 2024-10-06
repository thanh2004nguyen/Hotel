// Play audio
const playAudio = () => {
    // Create an AudioContext
    const audioContext = new (window.AudioContext || window.webkitAudioContext)();

    // url đến file âm thanh
    const audioFileUrl = '/audio/notice.wav'; 

    // Fetch the audio file
    fetch(audioFileUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.arrayBuffer();
        })
        .then(data => audioContext.decodeAudioData(data))
        .then(buffer => {
            const source = audioContext.createBufferSource();
            source.buffer = buffer;

            // Connect the source to the audio context's destination (speakers)
            source.connect(audioContext.destination);

            // Play the audio
            source.start(0);
        })
        .catch(error => {
            console.error('Error loading audio file:', error);
        });
};

// Show/hide dropdown menu
function showActionAccount() {
    var dropdownMenu = document.getElementById('dropdownMenu');
    dropdownMenu.classList.toggle('show');
}
