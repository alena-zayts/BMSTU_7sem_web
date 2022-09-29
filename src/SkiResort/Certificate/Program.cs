//using System.Security;
//using System.Security.Cryptography;
//using System.Security.Cryptography.X509Certificates;
//using System.Text;

////Генерируем ассиметричный ключ:


//var rsaKey = RSA.Create(2048);

/////Описываем субъект сертификации:


//string subject = "CN=localhost";

////cn – “common name”, общепринятое имя организации, определяющее, какие имена хостов будет защищать сертификат. В общем случае строка может содержать дополнительные поля, разделенные косой чертой. Пример из MSDN "C = US/O = Microsoft/OU = WGA/CN = test".


////Создаем запрос на сертификат:


//var certReq = new CertificateRequest(subject, rsaKey, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

////Режим Pkcs1 используется по умолчанию в openssl.


////Дополнительно настраиваем запрос:


//certReq.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
//certReq.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(certReq.PublicKey, false));

////Этими дополнениями мы указываем, что данный сертификат принадлежит центру сертификации, а также используем публичный ключ для создания SKI (идентификатора ключа субъекта).


////Создаем сертификат на 5 лет:


//var expirate = DateTimeOffset.Now.AddYears(5);
//var caCert = certReq.CreateSelfSigned(DateTimeOffset.Now, expirate);


////Теперь создаем конечные сертификаты.


////Запрос:


//var clientKey = RSA.Create(2048);
//string clisubject = "CN=10.10.10.*";
//var clientReq = new CertificateRequest(clisubject, clientKey, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

////Здесь в качестве защищаемого объекта можно указать подсеть или конечный ip адрес, если доменное имя не используется.


////В дополнение к предыдущим настройкам мы указываем назначение ключа:


//clientReq.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, false));
//clientReq.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation, false));
//clientReq.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(clientReq.PublicKey, false));

////Назначаем сертификату серийный номер:


//byte[] serialNumber = BitConverter.GetBytes(DateTime.Now.ToBinary());

////Serial Number – уникальный идентификатор, назначенный каждому сертификату, выданному издателем. Рекомендуется использовать общедоступный счетчик, инициализированный на основе текущего времени.


////Создаем сертификат на 5 лет:


//var clientCert = clientReq.Create(caCert, DateTimeOffset.Now, expirate, serialNumber);

////Время экспирации не должно выходить за границы корневого сертификата, иначе мы получим соответствующую ошибку. Теперь, если сравнить поля caCert.Subject и clientCert.Issuer, то они совпадают, как мы и ожидали.



////Хранение

////Теперь, когда у нас есть все сертификаты, необходимо их сохранить. В руководстве openssl обычно предлагается хранить сертификаты в формате pem, разбив их на открытую часть public.crt и private.key.Полученные на предыдущих этапах сертификаты являются экземплярами класса X509Certificate2, который имеет все необходимые для нас свойства и методы. Итак, чтобы получить открытую часть, необходимо закодировать содержимое сертификата в base64 и записать в файл:


//StringBuilder builder = new StringBuilder();
//builder.AppendLine("-----BEGIN CERTIFICATE-----");
//builder.AppendLine(Convert.ToBase64String(clientCert.RawData, Base64FormattingOptions.InsertLineBreaks));
//builder.AppendLine("-----END CERTIFICATE-----");
//File.WriteAllText("public.crt", builder.ToString());

////Также сохраним закрытый ключ:


//string name = clientKey.SignatureAlgorithm.ToUpper();
////StringBuilder builder = new StringBuilder();
//builder.AppendLine($"-----BEGIN {name} PRIVATE KEY-----");
//builder.AppendLine(Convert.ToBase64String(clientKey.ExportRSAPrivateKey(), Base64FormattingOptions.InsertLineBreaks));
//builder.AppendLine($"-----END {name} PRIVATE KEY-----");
//File.WriteAllText("private.key", builder.ToString());

////Отмечу, что метод ExportRSAPrivateKey появился только в netcore 3.0, поэтому в предыдущих версиях могут возникнуть проблемы.


////Еще один способ хранения сертификатов — формат pkcs12 или pfx, позволяющий уместить в одном файле открытую и закрытую часть. Класс X509Certificate2 содержит метод Export, принимающий в качестве аргумента необходимый нам ключ X509ContentType.Pkcs12 или X509ContentType.Pfx. Однако на этом этапе возникают трудности, поскольку загруженный обратно из файла сертификат неожиданно не содержит приватного ключа, о чем свидетельствует флаг cert.HasPrivateKey == false. Дело в том, что класс X509Certificate2 содержит внутреннюю метку, описывающую, куда и как экспортируется закрытый ключ. Эта метка инициализируется только с помощью конструктора и не может быть изменена после. Поэтому для создания файла p12 или pfx, необходимы дополнительные усилия:


//var exportCert = new X509Certificate2(clientCert.Export(X509ContentType.Cert), (string)null, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet).CopyWithPrivateKey(clientKey);
//File.WriteAllBytes("client.pfx", exportCert.Export(X509ContentType.Pfx));
//File.WriteAllBytes("client.p12", exportCert.Export(X509ContentType.Pkcs12));

////При желании можно использовать перегрузки метода Export с паролем.


////Для работы с коллекциями сертификатов в .net core удобно использовать специализированное хранилище, реализуемого классом X509Store. Хранилище в простом случае инициализируется строковым именем:


////X509Store store = new X509Store("test");
////store.Open(OpenFlags.ReadWrite);
////store.Add(cert);

////В результате для OC Linux будет создана папка "~/.dotnet/corefx/cryptography/x509stores/ test/" и файл "827...E3E.pfx", соответсвующий цифровому отпечатку (поле Thumbprint). Попытка что-то сделать с этим сертификатом с помощью openssl не увенчается успехом, так как он ожидаемо запаролен. Однако если положить в папку свой сертификат *.pfx без пароля, то он успешно подхватится хранилищем. К слову X509Store реализует методы поиска сертификатов по различным параметрам, включая IssueName и другие.
///



using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

// Generate private-public key pair
var rsaKey = RSA.Create(2048);

// Describe certificate
string subject = "CN=localhost";

// Create certificate request
var certificateRequest = new CertificateRequest(
    subject,
    rsaKey,
    HashAlgorithmName.SHA256,
    RSASignaturePadding.Pkcs1
);

certificateRequest.CertificateExtensions.Add(
    new X509BasicConstraintsExtension(
        certificateAuthority: false,
        hasPathLengthConstraint: false,
        pathLengthConstraint: 0,
        critical: true
    )
);

certificateRequest.CertificateExtensions.Add(
    new X509KeyUsageExtension(
        keyUsages:
            X509KeyUsageFlags.DigitalSignature
            | X509KeyUsageFlags.KeyEncipherment,
        critical: false
    )
);

certificateRequest.CertificateExtensions.Add(
    new X509SubjectKeyIdentifierExtension(
        key: certificateRequest.PublicKey,
        critical: false
    )
);

var expireAt = DateTimeOffset.Now.AddYears(5);

var certificate = certificateRequest.CreateSelfSigned(DateTimeOffset.Now, expireAt);

// Export certificate with private key
var exportableCertificate = new X509Certificate2(
    certificate.Export(X509ContentType.Cert),
    (string)null,
    X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet
).CopyWithPrivateKey(rsaKey);

exportableCertificate.FriendlyName = "Ivan Yakimov Test-only Certificate For Client Authorization";

// Create password for certificate protection
var passwordForCertificateProtection = new SecureString();
foreach (var @char in "test123")
{
    passwordForCertificateProtection.AppendChar(@char);
}

// Export certificate to a file.
File.WriteAllBytes(
    "certificateForClientAuthorization.pfx",
    exportableCertificate.Export(
        X509ContentType.Pfx,
        passwordForCertificateProtection
    )
);