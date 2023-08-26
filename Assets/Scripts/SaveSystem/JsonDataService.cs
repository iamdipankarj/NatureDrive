using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Solace {
  public class JsonDataService : IDataService {
    private const string KEY = "ggdPhkeOoiv6YMiPWa34kIuOdDUL7NwQFg6l1DVdwN8=";
    private const string IV = "JZuM0HQsWSBVpRHTeRZMYQ==";

    public bool SaveData<T>(string Path, T Data, bool Encrypted) {
      try {
        if (File.Exists(Path)) {
          Debug.Log("Data exists. Deleting old file and writing a new one!");
          File.Delete(Path);
        }
        else {
          Debug.Log("Writing file for the first time!");
        }
        using FileStream stream = File.Create(Path);
        if (Encrypted) {
          WriteEncryptedData(Data, stream);
        } else {
          stream.Close();
          File.WriteAllText(Path, JsonConvert.SerializeObject(Data, Formatting.Indented));
        }
        return true;
      } catch (Exception e) {
        Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
        return false;
      }
    }

    private void WriteEncryptedData<T>(T Data, FileStream Stream) {
      using Aes aesProvider = Aes.Create();
      aesProvider.Key = Convert.FromBase64String(KEY);
      aesProvider.IV = Convert.FromBase64String(IV);
      using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
      using CryptoStream cryptoStream = new(
        Stream,
        cryptoTransform,
        CryptoStreamMode.Write
      );
      cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(Data)));
    }

    public T LoadData<T>(string Path, bool Encrypted) {
      if (!File.Exists(Path)) {
        return default;
      }
      try {
        T data;
        if (Encrypted) {
          data = ReadEncryptedData<T>(Path);
        } else {
          data = JsonConvert.DeserializeObject<T>(File.ReadAllText(Path));
        }
        return data;
      }
      catch (Exception e) {
        Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
        throw e;
      }
    }

    private T ReadEncryptedData<T>(string Path) {
      byte[] fileBytes = File.ReadAllBytes(Path);
      using Aes aesProvider = Aes.Create();

      aesProvider.Key = Convert.FromBase64String(KEY);
      aesProvider.IV = Convert.FromBase64String(IV);

      using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(
          aesProvider.Key,
          aesProvider.IV
      );
      using MemoryStream decryptionStream = new MemoryStream(fileBytes);
      using CryptoStream cryptoStream = new CryptoStream(
          decryptionStream,
          cryptoTransform,
          CryptoStreamMode.Read
      );
      using StreamReader reader = new StreamReader(cryptoStream);

      string result = reader.ReadToEnd();

      Debug.Log($"Decrypted result (if the following is not legible, probably wrong key or iv): {result}");
      return JsonConvert.DeserializeObject<T>(result);
    }
  }
}
