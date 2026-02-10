# Projedeki Eksikler

1. **API uç noktaları eksik**: `Application` katmanında Orders, Products ve Categories için çok sayıda command/query olmasına rağmen API katmanında sadece `WeatherForecastController` bulunuyor. Bu nedenle işlevler dışarı açılmamış durumda.
2. **Demo/şablon kod hâlâ duruyor**: `WeatherForecast` modeli ve `WeatherForecastController` gerçek domain yerine template kodu olarak kalmış.
3. **API örnek istek dosyası güncel değil**: `.http` dosyasında sadece `/weatherforecast` isteği var; gerçek endpoint örnekleri yok.
4. **Test projesi yok**: Çözümde birim/entegrasyon testi için ayrı bir test csproj bulunmuyor.
5. **SMS entegrasyonu tamamlanmamış**: `SmsNotificationSender` sınıfı gerçek SMS sağlayıcısına bağlanmıyor; TODO olarak bırakılmış.
6. **Build doğrulaması yapılamadı (ortam eksiği)**: Bu ortamda `dotnet` komutu yüklü olmadığı için derleme/test çalıştırılamıyor.
