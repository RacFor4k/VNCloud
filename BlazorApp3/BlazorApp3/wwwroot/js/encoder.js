// Пример шифрования с явным указанием IV (соли)

function bytesToWordArray(bytes) {
    const words = [];
    for (let i = 0; i < bytes.length; i += 4) {
        words.push(
            (bytes[i] << 24) | (bytes[i + 1] << 16) | (bytes[i + 2] << 8) | bytes[i + 3]
        );
    }
    return CryptoJS.lib.WordArray.create(words, bytes.length);
}

window.encryptAES = (dataBytes, keyBytes, ivBytes) => {
    const dataWordArray = bytesToWordArray(dataBytes);
    const keyWordArray = bytesToWordArray(keyBytes);
    const ivWordArray = bytesToWordArray(ivBytes);
    const encrypted = CryptoJS.AES.encrypt(dataWordArray, keyWordArray, {
        iv: ivWordArray,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });
    return encrypted.toString();
}

// Пример дешифрования с явным указанием IV (соли)
window.decryptAES = (encryptedData, keyBytes, ivBytes) => {
    const keyWordArray = bytesToWordArray(keyBytes);
    const ivWordArray = bytesToWordArray(ivBytes);
    const decrypted = CryptoJS.AES.decrypt(encryptedData, keyWordArray, {
        iv: ivWordArray,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });
    return decrypted.toString(CryptoJS.enc.Utf8);
}
