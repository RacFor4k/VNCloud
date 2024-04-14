function encryptAES(byteArray, key) {
    // Используйте библиотеку CryptoJS для шифрования
    var wordArray = CryptoJS.lib.WordArray.create(byteArray);
    var encrypted = CryptoJS.AES.encrypt(wordArray, key).toString();
    return encrypted;
}

function decryptAES(encryptedData, key) {
    // Используйте библиотеку CryptoJS для расшифровывания
    var decrypted = CryptoJS.AES.decrypt(encryptedData, key);
    var typedArray = new Uint8Array(decrypted.words.length * 4);
    var offset = 0;
    decrypted.words.forEach(function (word) {
        typedArray[offset++] = (word >> 24) & 0xff;
        typedArray[offset++] = (word >> 16) & 0xff;
        typedArray[offset++] = (word >> 8) & 0xff;
        typedArray[offset++] = word & 0xff;
    });
    return typedArray;
}
