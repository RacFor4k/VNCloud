window.encryptAES = (byteArray, key) => {
    // Используйте библиотеку CryptoJS для шифрования
    var wordArray = CryptoJS.lib.WordArray.create(byteArray);
    var iv = CryptoJS.enc.Hex.parse('000102030405060708090a0b0c0d0e0f'); // Статический IV
    var encrypted = CryptoJS.AES.encrypt(wordArray, key, { iv: iv }).toString();
    return encrypted;
}

window.decryptAES = (encryptedData, key) => {
    // Используйте библиотеку CryptoJS для расшифровывания
    var iv = CryptoJS.enc.Hex.parse('000102030405060708090a0b0c0d0e0f'); // Тот же статический IV
    var decrypted = CryptoJS.AES.decrypt(encryptedData, key, { iv: iv });
    var typedArray = new Uint8Array(decrypted.sigBytes);
    decrypted.words.forEach((word, index) => {
        typedArray[index * 4] = (word >> 24) & 0xff;
        typedArray[index * 4 + 1] = (word >> 16) & 0xff;
        typedArray[index * 4 + 2] = (word >> 8) & 0xff;
        typedArray[index * 4 + 3] = word & 0xff;
    });
    return typedArray;
}
