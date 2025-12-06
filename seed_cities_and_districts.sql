-- Türkiye'nin 81 İli ve İlçeleri Seed Data
-- PostgreSQL için hazırlanmıştır

-- Önce tüm şehirleri ekle
INSERT INTO "Cities" ("Name", "CreatedDate", "UpdatedDate")
VALUES
    ('Adana', NOW(), NULL),
    ('Adıyaman', NOW(), NULL),
    ('Afyonkarahisar', NOW(), NULL),
    ('Ağrı', NOW(), NULL),
    ('Aksaray', NOW(), NULL),
    ('Amasya', NOW(), NULL),
    ('Ankara', NOW(), NULL),
    ('Antalya', NOW(), NULL),
    ('Ardahan', NOW(), NULL),
    ('Artvin', NOW(), NULL),
    ('Aydın', NOW(), NULL),
    ('Balıkesir', NOW(), NULL),
    ('Bartın', NOW(), NULL),
    ('Batman', NOW(), NULL),
    ('Bayburt', NOW(), NULL),
    ('Bilecik', NOW(), NULL),
    ('Bingöl', NOW(), NULL),
    ('Bitlis', NOW(), NULL),
    ('Bolu', NOW(), NULL),
    ('Burdur', NOW(), NULL),
    ('Bursa', NOW(), NULL),
    ('Çanakkale', NOW(), NULL),
    ('Çankırı', NOW(), NULL),
    ('Çorum', NOW(), NULL),
    ('Denizli', NOW(), NULL),
    ('Diyarbakır', NOW(), NULL),
    ('Düzce', NOW(), NULL),
    ('Edirne', NOW(), NULL),
    ('Elazığ', NOW(), NULL),
    ('Erzincan', NOW(), NULL),
    ('Erzurum', NOW(), NULL),
    ('Eskişehir', NOW(), NULL),
    ('Gaziantep', NOW(), NULL),
    ('Giresun', NOW(), NULL),
    ('Gümüşhane', NOW(), NULL),
    ('Hakkari', NOW(), NULL),
    ('Hatay', NOW(), NULL),
    ('Iğdır', NOW(), NULL),
    ('Isparta', NOW(), NULL),
    ('İstanbul', NOW(), NULL),
    ('İzmir', NOW(), NULL),
    ('Kahramanmaraş', NOW(), NULL),
    ('Karabük', NOW(), NULL),
    ('Karaman', NOW(), NULL),
    ('Kars', NOW(), NULL),
    ('Kastamonu', NOW(), NULL),
    ('Kayseri', NOW(), NULL),
    ('Kırıkkale', NOW(), NULL),
    ('Kırklareli', NOW(), NULL),
    ('Kırşehir', NOW(), NULL),
    ('Kilis', NOW(), NULL),
    ('Kocaeli', NOW(), NULL),
    ('Konya', NOW(), NULL),
    ('Kütahya', NOW(), NULL),
    ('Malatya', NOW(), NULL),
    ('Manisa', NOW(), NULL),
    ('Mardin', NOW(), NULL),
    ('Mersin', NOW(), NULL),
    ('Muğla', NOW(), NULL),
    ('Muş', NOW(), NULL),
    ('Nevşehir', NOW(), NULL),
    ('Niğde', NOW(), NULL),
    ('Ordu', NOW(), NULL),
    ('Osmaniye', NOW(), NULL),
    ('Rize', NOW(), NULL),
    ('Sakarya', NOW(), NULL),
    ('Samsun', NOW(), NULL),
    ('Siirt', NOW(), NULL),
    ('Sinop', NOW(), NULL),
    ('Sivas', NOW(), NULL),
    ('Şanlıurfa', NOW(), NULL),
    ('Şırnak', NOW(), NULL),
    ('Tekirdağ', NOW(), NULL),
    ('Tokat', NOW(), NULL),
    ('Trabzon', NOW(), NULL),
    ('Tunceli', NOW(), NULL),
    ('Uşak', NOW(), NULL),
    ('Van', NOW(), NULL),
    ('Yalova', NOW(), NULL),
    ('Yozgat', NOW(), NULL),
    ('Zonguldak', NOW(), NULL)
ON CONFLICT DO NOTHING;

-- İlçeleri ekle (şehir isimlerine göre)
INSERT INTO "Districts" ("CityId", "Name", "CreatedDate", "UpdatedDate")
SELECT c."Id", d."Name", NOW(), NULL
FROM (VALUES
    -- Adana
    ('Adana', 'Aladağ'), ('Adana', 'Ceyhan'), ('Adana', 'Çukurova'), ('Adana', 'Feke'), 
    ('Adana', 'İmamoğlu'), ('Adana', 'Karaisalı'), ('Adana', 'Karataş'), ('Adana', 'Kozan'), 
    ('Adana', 'Pozantı'), ('Adana', 'Saimbeyli'), ('Adana', 'Sarıçam'), ('Adana', 'Seyhan'), 
    ('Adana', 'Tufanbeyli'), ('Adana', 'Yumurtalık'), ('Adana', 'Yüreğir'),
    
    -- Adıyaman
    ('Adıyaman', 'Adıyaman'), ('Adıyaman', 'Besni'), ('Adıyaman', 'Çelikhan'), ('Adıyaman', 'Gerger'), 
    ('Adıyaman', 'Gölbaşı'), ('Adıyaman', 'Kâhta'), ('Adıyaman', 'Samsat'), ('Adıyaman', 'Sincik'), 
    ('Adıyaman', 'Tut'),
    
    -- Afyonkarahisar
    ('Afyonkarahisar', 'Afyonkarahisar'), ('Afyonkarahisar', 'Başmakçı'), ('Afyonkarahisar', 'Bayat'), 
    ('Afyonkarahisar', 'Bolvadin'), ('Afyonkarahisar', 'Çay'), ('Afyonkarahisar', 'Çobanlar'), 
    ('Afyonkarahisar', 'Dazkırı'), ('Afyonkarahisar', 'Dinar'), ('Afyonkarahisar', 'Emirdağ'), 
    ('Afyonkarahisar', 'Evciler'), ('Afyonkarahisar', 'Hocalar'), ('Afyonkarahisar', 'İhsaniye'), 
    ('Afyonkarahisar', 'İscehisar'), ('Afyonkarahisar', 'Kızılören'), ('Afyonkarahisar', 'Sandıklı'), 
    ('Afyonkarahisar', 'Sinanpaşa'), ('Afyonkarahisar', 'Sultandağı'), ('Afyonkarahisar', 'Şuhut'),
    
    -- Ağrı
    ('Ağrı', 'Ağrı'), ('Ağrı', 'Diyadin'), ('Ağrı', 'Doğubayazıt'), ('Ağrı', 'Eleşkirt'), 
    ('Ağrı', 'Hamur'), ('Ağrı', 'Patnos'), ('Ağrı', 'Taşlıçay'), ('Ağrı', 'Tutak'),
    
    -- Aksaray
    ('Aksaray', 'Aksaray'), ('Aksaray', 'Ağaçören'), ('Aksaray', 'Eskil'), ('Aksaray', 'Gülağaç'), 
    ('Aksaray', 'Güzelyurt'), ('Aksaray', 'Ortaköy'), ('Aksaray', 'Sarıyahşi'), ('Aksaray', 'Sultanhanı'),
    
    -- Amasya
    ('Amasya', 'Amasya'), ('Amasya', 'Göynücek'), ('Amasya', 'Gümüşhacıköy'), ('Amasya', 'Hamamözü'), 
    ('Amasya', 'Merzifon'), ('Amasya', 'Suluova'), ('Amasya', 'Taşova'),
    
    -- Ankara
    ('Ankara', 'Akyurt'), ('Ankara', 'Altındağ'), ('Ankara', 'Ayaş'), ('Ankara', 'Balâ'), 
    ('Ankara', 'Beypazarı'), ('Ankara', 'Çamlıdere'), ('Ankara', 'Çankaya'), ('Ankara', 'Çubuk'), 
    ('Ankara', 'Elmadağ'), ('Ankara', 'Etimesgut'), ('Ankara', 'Evren'), ('Ankara', 'Gölbaşı'), 
    ('Ankara', 'Güdül'), ('Ankara', 'Haymana'), ('Ankara', 'Kahramankazan'), ('Ankara', 'Kalecik'), 
    ('Ankara', 'Keçiören'), ('Ankara', 'Kızılcahamam'), ('Ankara', 'Mamak'), ('Ankara', 'Nallıhan'), 
    ('Ankara', 'Polatlı'), ('Ankara', 'Pursaklar'), ('Ankara', 'Sincan'), ('Ankara', 'Şereflikoçhisar'), 
    ('Ankara', 'Yenimahalle'),
    
    -- Antalya
    ('Antalya', 'Akseki'), ('Antalya', 'Aksu'), ('Antalya', 'Alanya'), ('Antalya', 'Demre'), 
    ('Antalya', 'Döşemealtı'), ('Antalya', 'Elmalı'), ('Antalya', 'Finike'), ('Antalya', 'Gazipaşa'), 
    ('Antalya', 'Gündoğmuş'), ('Antalya', 'İbradı'), ('Antalya', 'Kaş'), ('Antalya', 'Kemer'), 
    ('Antalya', 'Kepez'), ('Antalya', 'Konyaaltı'), ('Antalya', 'Korkuteli'), ('Antalya', 'Kumluca'), 
    ('Antalya', 'Manavgat'), ('Antalya', 'Muratpaşa'), ('Antalya', 'Serik'),
    
    -- Ardahan
    ('Ardahan', 'Ardahan'), ('Ardahan', 'Çıldır'), ('Ardahan', 'Damal'), ('Ardahan', 'Göle'), 
    ('Ardahan', 'Hanak'), ('Ardahan', 'Posof'),
    
    -- Artvin
    ('Artvin', 'Artvin'), ('Artvin', 'Ardanuç'), ('Artvin', 'Arhavi'), ('Artvin', 'Borçka'), 
    ('Artvin', 'Hopa'), ('Artvin', 'Kemalpaşa'), ('Artvin', 'Murgul'), ('Artvin', 'Şavşat'), 
    ('Artvin', 'Yusufeli'),
    
    -- Aydın
    ('Aydın', 'Bozdoğan'), ('Aydın', 'Buharkent'), ('Aydın', 'Çine'), ('Aydın', 'Didim'), 
    ('Aydın', 'Efeler'), ('Aydın', 'Germencik'), ('Aydın', 'İncirliova'), ('Aydın', 'Karacasu'), 
    ('Aydın', 'Karpuzlu'), ('Aydın', 'Koçarlı'), ('Aydın', 'Köşk'), ('Aydın', 'Kuşadası'), 
    ('Aydın', 'Kuyucak'), ('Aydın', 'Nazilli'), ('Aydın', 'Söke'), ('Aydın', 'Sultanhisar'), 
    ('Aydın', 'Yenipazar'),
    
    -- Balıkesir
    ('Balıkesir', 'Altıeylül'), ('Balıkesir', 'Ayvalık'), ('Balıkesir', 'Balya'), ('Balıkesir', 'Bandırma'), 
    ('Balıkesir', 'Bigadiç'), ('Balıkesir', 'Burhaniye'), ('Balıkesir', 'Dursunbey'), ('Balıkesir', 'Edremit'), 
    ('Balıkesir', 'Erdek'), ('Balıkesir', 'Gömeç'), ('Balıkesir', 'Gönen'), ('Balıkesir', 'Havran'), 
    ('Balıkesir', 'İvrindi'), ('Balıkesir', 'Karesi'), ('Balıkesir', 'Kepsut'), ('Balıkesir', 'Manyas'), 
    ('Balıkesir', 'Marmara'), ('Balıkesir', 'Savaştepe'), ('Balıkesir', 'Sındırgı'), ('Balıkesir', 'Susurluk'),
    
    -- Bartın
    ('Bartın', 'Bartın'), ('Bartın', 'Amasra'), ('Bartın', 'Kurucaşile'), ('Bartın', 'Ulus'),
    
    -- Batman
    ('Batman', 'Batman'), ('Batman', 'Beşiri'), ('Batman', 'Gercüş'), ('Batman', 'Hasankeyf'), 
    ('Batman', 'Kozluk'), ('Batman', 'Sason'),
    
    -- Bayburt
    ('Bayburt', 'Bayburt'), ('Bayburt', 'Aydıntepe'), ('Bayburt', 'Demirözü'),
    
    -- Bilecik
    ('Bilecik', 'Bilecik'), ('Bilecik', 'Bozüyük'), ('Bilecik', 'Gölpazarı'), ('Bilecik', 'İnhisar'), 
    ('Bilecik', 'Osmaneli'), ('Bilecik', 'Pazaryeri'), ('Bilecik', 'Söğüt'), ('Bilecik', 'Yenipazar'),
    
    -- Bingöl
    ('Bingöl', 'Bingöl'), ('Bingöl', 'Adaklı'), ('Bingöl', 'Genç'), ('Bingöl', 'Karlıova'), 
    ('Bingöl', 'Kiğı'), ('Bingöl', 'Solhan'), ('Bingöl', 'Yayladere'), ('Bingöl', 'Yedisu'),
    
    -- Bitlis
    ('Bitlis', 'Bitlis'), ('Bitlis', 'Adilcevaz'), ('Bitlis', 'Ahlat'), ('Bitlis', 'Güroymak'), 
    ('Bitlis', 'Hizan'), ('Bitlis', 'Mutki'), ('Bitlis', 'Tatvan'),
    
    -- Bolu
    ('Bolu', 'Bolu'), ('Bolu', 'Dörtdivan'), ('Bolu', 'Gerede'), ('Bolu', 'Göynük'), 
    ('Bolu', 'Kıbrıscık'), ('Bolu', 'Mengen'), ('Bolu', 'Mudurnu'), ('Bolu', 'Seben'), 
    ('Bolu', 'Yeniçağa'),
    
    -- Burdur
    ('Burdur', 'Burdur'), ('Burdur', 'Ağlasun'), ('Burdur', 'Altınyayla'), ('Burdur', 'Bucak'), 
    ('Burdur', 'Çavdır'), ('Burdur', 'Çeltikçi'), ('Burdur', 'Gölhisar'), ('Burdur', 'Karamanlı'), 
    ('Burdur', 'Kemer'), ('Burdur', 'Tefenni'), ('Burdur', 'Yeşilova'),
    
    -- Bursa
    ('Bursa', 'Büyükorhan'), ('Bursa', 'Gemlik'), ('Bursa', 'Gürsu'), ('Bursa', 'Harmancık'), 
    ('Bursa', 'İnegöl'), ('Bursa', 'İznik'), ('Bursa', 'Karacabey'), ('Bursa', 'Keles'), 
    ('Bursa', 'Kestel'), ('Bursa', 'Mudanya'), ('Bursa', 'Mustafakemalpaşa'), ('Bursa', 'Nilüfer'), 
    ('Bursa', 'Orhaneli'), ('Bursa', 'Orhangazi'), ('Bursa', 'Osmangazi'), ('Bursa', 'Yenişehir'), 
    ('Bursa', 'Yıldırım'),
    
    -- Çanakkale
    ('Çanakkale', 'Çanakkale'), ('Çanakkale', 'Ayvacık'), ('Çanakkale', 'Bayramiç'), ('Çanakkale', 'Biga'), 
    ('Çanakkale', 'Bozcaada'), ('Çanakkale', 'Çan'), ('Çanakkale', 'Eceabat'), ('Çanakkale', 'Ezine'), 
    ('Çanakkale', 'Gelibolu'), ('Çanakkale', 'Gökçeada'), ('Çanakkale', 'Lapseki'), ('Çanakkale', 'Yenice'),
    
    -- Çankırı
    ('Çankırı', 'Çankırı'), ('Çankırı', 'Atkaracalar'), ('Çankırı', 'Bayramören'), ('Çankırı', 'Çerkeş'), 
    ('Çankırı', 'Eldivan'), ('Çankırı', 'Ilgaz'), ('Çankırı', 'Kızılırmak'), ('Çankırı', 'Korgun'), 
    ('Çankırı', 'Kurşunlu'), ('Çankırı', 'Orta'), ('Çankırı', 'Şabanözü'), ('Çankırı', 'Yapraklı'),
    
    -- Çorum
    ('Çorum', 'Çorum'), ('Çorum', 'Alaca'), ('Çorum', 'Bayat'), ('Çorum', 'Boğazkale'), 
    ('Çorum', 'Dodurga'), ('Çorum', 'İskilip'), ('Çorum', 'Kargı'), ('Çorum', 'Laçin'), 
    ('Çorum', 'Mecitözü'), ('Çorum', 'Oğuzlar'), ('Çorum', 'Ortaköy'), ('Çorum', 'Osmancık'), 
    ('Çorum', 'Sungurlu'), ('Çorum', 'Uğurludağ'),
    
    -- Denizli
    ('Denizli', 'Acıpayam'), ('Denizli', 'Babadağ'), ('Denizli', 'Baklan'), ('Denizli', 'Bekilli'), 
    ('Denizli', 'Beyağaç'), ('Denizli', 'Bozkurt'), ('Denizli', 'Buldan'), ('Denizli', 'Çal'), 
    ('Denizli', 'Çameli'), ('Denizli', 'Çardak'), ('Denizli', 'Çivril'), ('Denizli', 'Güney'), 
    ('Denizli', 'Honaz'), ('Denizli', 'Kale'), ('Denizli', 'Merkezefendi'), ('Denizli', 'Pamukkale'), 
    ('Denizli', 'Sarayköy'), ('Denizli', 'Serinhisar'), ('Denizli', 'Tavas'),
    
    -- Diyarbakır
    ('Diyarbakır', 'Bağlar'), ('Diyarbakır', 'Bismil'), ('Diyarbakır', 'Çermik'), ('Diyarbakır', 'Çınar'), 
    ('Diyarbakır', 'Çüngüş'), ('Diyarbakır', 'Dicle'), ('Diyarbakır', 'Eğil'), ('Diyarbakır', 'Ergani'), 
    ('Diyarbakır', 'Hani'), ('Diyarbakır', 'Hazro'), ('Diyarbakır', 'Kayapınar'), ('Diyarbakır', 'Kocaköy'), 
    ('Diyarbakır', 'Kulp'), ('Diyarbakır', 'Lice'), ('Diyarbakır', 'Silvan'), ('Diyarbakır', 'Sur'), 
    ('Diyarbakır', 'Yenişehir'),
    
    -- Düzce
    ('Düzce', 'Düzce'), ('Düzce', 'Akçakoca'), ('Düzce', 'Cumayeri'), ('Düzce', 'Çilimli'), 
    ('Düzce', 'Gölyaka'), ('Düzce', 'Gümüşova'), ('Düzce', 'Kaynaşlı'), ('Düzce', 'Yığılca'),
    
    -- Edirne
    ('Edirne', 'Edirne'), ('Edirne', 'Enez'), ('Edirne', 'Havsa'), ('Edirne', 'İpsala'), 
    ('Edirne', 'Keşan'), ('Edirne', 'Lalapaşa'), ('Edirne', 'Meriç'), ('Edirne', 'Süloğlu'), 
    ('Edirne', 'Uzunköprü'),
    
    -- Elazığ
    ('Elazığ', 'Elazığ'), ('Elazığ', 'Ağın'), ('Elazığ', 'Alacakaya'), ('Elazığ', 'Arıcak'), 
    ('Elazığ', 'Baskil'), ('Elazığ', 'Karakoçan'), ('Elazığ', 'Keban'), ('Elazığ', 'Kovancılar'), 
    ('Elazığ', 'Maden'), ('Elazığ', 'Palu'), ('Elazığ', 'Sivrice'),
    
    -- Erzincan
    ('Erzincan', 'Erzincan'), ('Erzincan', 'Çayırlı'), ('Erzincan', 'İliç'), ('Erzincan', 'Kemah'), 
    ('Erzincan', 'Kemaliye'), ('Erzincan', 'Otlukbeli'), ('Erzincan', 'Refahiye'), ('Erzincan', 'Tercan'), 
    ('Erzincan', 'Üzümlü'),
    
    -- Erzurum
    ('Erzurum', 'Aşkale'), ('Erzurum', 'Aziziye'), ('Erzurum', 'Çat'), ('Erzurum', 'Hınıs'), 
    ('Erzurum', 'Horasan'), ('Erzurum', 'İspir'), ('Erzurum', 'Karaçoban'), ('Erzurum', 'Karayazı'), 
    ('Erzurum', 'Köprüköy'), ('Erzurum', 'Narman'), ('Erzurum', 'Oltu'), ('Erzurum', 'Olur'), 
    ('Erzurum', 'Palandöken'), ('Erzurum', 'Pasinler'), ('Erzurum', 'Pazaryolu'), ('Erzurum', 'Şenkaya'), 
    ('Erzurum', 'Tekman'), ('Erzurum', 'Tortum'), ('Erzurum', 'Uzundere'), ('Erzurum', 'Yakutiye'),
    
    -- Eskişehir
    ('Eskişehir', 'Alpu'), ('Eskişehir', 'Beylikova'), ('Eskişehir', 'Çifteler'), ('Eskişehir', 'Günyüzü'), 
    ('Eskişehir', 'Han'), ('Eskişehir', 'İnönü'), ('Eskişehir', 'Mahmudiye'), ('Eskişehir', 'Mihalgazi'), 
    ('Eskişehir', 'Mihalıççık'), ('Eskişehir', 'Odunpazarı'), ('Eskişehir', 'Sarıcakaya'), ('Eskişehir', 'Seyitgazi'), 
    ('Eskişehir', 'Sivrihisar'), ('Eskişehir', 'Tepebaşı'),
    
    -- Gaziantep
    ('Gaziantep', 'Araban'), ('Gaziantep', 'İslahiye'), ('Gaziantep', 'Karkamış'), ('Gaziantep', 'Nizip'), 
    ('Gaziantep', 'Nurdağı'), ('Gaziantep', 'Oğuzeli'), ('Gaziantep', 'Şahinbey'), ('Gaziantep', 'Şehitkamil'), 
    ('Gaziantep', 'Yavuzeli'),
    
    -- Giresun
    ('Giresun', 'Giresun'), ('Giresun', 'Alucra'), ('Giresun', 'Bulancak'), ('Giresun', 'Çamoluk'), 
    ('Giresun', 'Çanakçı'), ('Giresun', 'Dereli'), ('Giresun', 'Doğankent'), ('Giresun', 'Espiye'), 
    ('Giresun', 'Eynesil'), ('Giresun', 'Görele'), ('Giresun', 'Güce'), ('Giresun', 'Keşap'), 
    ('Giresun', 'Piraziz'), ('Giresun', 'Şebinkarahisar'), ('Giresun', 'Tirebolu'), ('Giresun', 'Yağlıdere'),
    
    -- Gümüşhane
    ('Gümüşhane', 'Gümüşhane'), ('Gümüşhane', 'Kelkit'), ('Gümüşhane', 'Köse'), ('Gümüşhane', 'Kürtün'), 
    ('Gümüşhane', 'Şiran'), ('Gümüşhane', 'Torul'),
    
    -- Hakkari
    ('Hakkari', 'Hakkari'), ('Hakkari', 'Çukurca'), ('Hakkari', 'Derecik'), ('Hakkari', 'Şemdinli'), 
    ('Hakkari', 'Yüksekova'),
    
    -- Hatay
    ('Hatay', 'Altınözü'), ('Hatay', 'Antakya'), ('Hatay', 'Arsuz'), ('Hatay', 'Belen'), 
    ('Hatay', 'Defne'), ('Hatay', 'Dörtyol'), ('Hatay', 'Erzin'), ('Hatay', 'Hassa'), 
    ('Hatay', 'İskenderun'), ('Hatay', 'Kırıkhan'), ('Hatay', 'Kumlu'), ('Hatay', 'Payas'), 
    ('Hatay', 'Reyhanlı'), ('Hatay', 'Samandağ'), ('Hatay', 'Yayladağı'),
    
    -- Iğdır
    ('Iğdır', 'Iğdır'), ('Iğdır', 'Aralık'), ('Iğdır', 'Karakoyunlu'), ('Iğdır', 'Tuzluca'),
    
    -- Isparta
    ('Isparta', 'Isparta'), ('Isparta', 'Aksu'), ('Isparta', 'Atabey'), ('Isparta', 'Eğirdir'), 
    ('Isparta', 'Gelendost'), ('Isparta', 'Gönen'), ('Isparta', 'Keçiborlu'), ('Isparta', 'Senirkent'), 
    ('Isparta', 'Sütçüler'), ('Isparta', 'Şarkikaraağaç'), ('Isparta', 'Uluborlu'), ('Isparta', 'Yalvaç'), 
    ('Isparta', 'Yenişarbademli'),
    
    -- İstanbul
    ('İstanbul', 'Adalar'), ('İstanbul', 'Arnavutköy'), ('İstanbul', 'Ataşehir'), ('İstanbul', 'Avcılar'), 
    ('İstanbul', 'Bağcılar'), ('İstanbul', 'Bahçelievler'), ('İstanbul', 'Bakırköy'), ('İstanbul', 'Başakşehir'), 
    ('İstanbul', 'Bayrampaşa'), ('İstanbul', 'Beşiktaş'), ('İstanbul', 'Beykoz'), ('İstanbul', 'Beylikdüzü'), 
    ('İstanbul', 'Beyoğlu'), ('İstanbul', 'Büyükçekmece'), ('İstanbul', 'Çatalca'), ('İstanbul', 'Çekmeköy'), 
    ('İstanbul', 'Esenler'), ('İstanbul', 'Esenyurt'), ('İstanbul', 'Eyüpsultan'), ('İstanbul', 'Fatih'), 
    ('İstanbul', 'Gaziosmanpaşa'), ('İstanbul', 'Güngören'), ('İstanbul', 'Kadıköy'), ('İstanbul', 'Kâğıthane'), 
    ('İstanbul', 'Kartal'), ('İstanbul', 'Küçükçekmece'), ('İstanbul', 'Maltepe'), ('İstanbul', 'Pendik'), 
    ('İstanbul', 'Sancaktepe'), ('İstanbul', 'Sarıyer'), ('İstanbul', 'Silivri'), ('İstanbul', 'Sultanbeyli'), 
    ('İstanbul', 'Sultangazi'), ('İstanbul', 'Şile'), ('İstanbul', 'Şişli'), ('İstanbul', 'Tuzla'), 
    ('İstanbul', 'Ümraniye'), ('İstanbul', 'Üsküdar'), ('İstanbul', 'Zeytinburnu'),
    
    -- İzmir
    ('İzmir', 'Aliağa'), ('İzmir', 'Balçova'), ('İzmir', 'Bayındır'), ('İzmir', 'Bayraklı'), 
    ('İzmir', 'Bergama'), ('İzmir', 'Beydağ'), ('İzmir', 'Bornova'), ('İzmir', 'Buca'), 
    ('İzmir', 'Çeşme'), ('İzmir', 'Çiğli'), ('İzmir', 'Dikili'), ('İzmir', 'Foça'), 
    ('İzmir', 'Gaziemir'), ('İzmir', 'Güzelbahçe'), ('İzmir', 'Karabağlar'), ('İzmir', 'Karaburun'), 
    ('İzmir', 'Karşıyaka'), ('İzmir', 'Kemalpaşa'), ('İzmir', 'Kınık'), ('İzmir', 'Kiraz'), 
    ('İzmir', 'Konak'), ('İzmir', 'Menderes'), ('İzmir', 'Menemen'), ('İzmir', 'Narlıdere'), 
    ('İzmir', 'Ödemiş'), ('İzmir', 'Seferihisar'), ('İzmir', 'Selçuk'), ('İzmir', 'Tire'), 
    ('İzmir', 'Torbalı'), ('İzmir', 'Urla'),
    
    -- Kahramanmaraş
    ('Kahramanmaraş', 'Afşin'), ('Kahramanmaraş', 'Andırın'), ('Kahramanmaraş', 'Çağlayancerit'), 
    ('Kahramanmaraş', 'Dulkadiroğlu'), ('Kahramanmaraş', 'Ekinözü'), ('Kahramanmaraş', 'Elbistan'), 
    ('Kahramanmaraş', 'Göksun'), ('Kahramanmaraş', 'Nurhak'), ('Kahramanmaraş', 'Onikişubat'), 
    ('Kahramanmaraş', 'Pazarcık'), ('Kahramanmaraş', 'Türkoğlu'),
    
    -- Karabük
    ('Karabük', 'Karabük'), ('Karabük', 'Eflani'), ('Karabük', 'Eskipazar'), ('Karabük', 'Ovacık'), 
    ('Karabük', 'Safranbolu'), ('Karabük', 'Yenice'),
    
    -- Karaman
    ('Karaman', 'Karaman'), ('Karaman', 'Ayrancı'), ('Karaman', 'Başyayla'), ('Karaman', 'Ermenek'), 
    ('Karaman', 'Kazımkarabekir'), ('Karaman', 'Sarıveliler'),
    
    -- Kars
    ('Kars', 'Kars'), ('Kars', 'Akyaka'), ('Kars', 'Arpaçay'), ('Kars', 'Digor'), 
    ('Kars', 'Kağızman'), ('Kars', 'Sarıkamış'), ('Kars', 'Selim'), ('Kars', 'Susuz'),
    
    -- Kastamonu
    ('Kastamonu', 'Kastamonu'), ('Kastamonu', 'Abana'), ('Kastamonu', 'Ağlı'), ('Kastamonu', 'Araç'), 
    ('Kastamonu', 'Azdavay'), ('Kastamonu', 'Bozkurt'), ('Kastamonu', 'Cide'), ('Kastamonu', 'Çatalzeytin'), 
    ('Kastamonu', 'Daday'), ('Kastamonu', 'Devrekani'), ('Kastamonu', 'Doğanyurt'), ('Kastamonu', 'Hanönü'), 
    ('Kastamonu', 'İhsangazi'), ('Kastamonu', 'İnebolu'), ('Kastamonu', 'Küre'), ('Kastamonu', 'Pınarbaşı'), 
    ('Kastamonu', 'Seydiler'), ('Kastamonu', 'Şenpazar'), ('Kastamonu', 'Taşköprü'), ('Kastamonu', 'Tosya'),
    
    -- Kayseri
    ('Kayseri', 'Akkışla'), ('Kayseri', 'Bünyan'), ('Kayseri', 'Develi'), ('Kayseri', 'Felahiye'), 
    ('Kayseri', 'Hacılar'), ('Kayseri', 'İncesu'), ('Kayseri', 'Kocasinan'), ('Kayseri', 'Melikgazi'), 
    ('Kayseri', 'Özvatan'), ('Kayseri', 'Pınarbaşı'), ('Kayseri', 'Sarıoğlan'), ('Kayseri', 'Sarız'), 
    ('Kayseri', 'Talas'), ('Kayseri', 'Tomarza'), ('Kayseri', 'Yahyalı'), ('Kayseri', 'Yeşilhisar'),
    
    -- Kırıkkale
    ('Kırıkkale', 'Kırıkkale'), ('Kırıkkale', 'Bahşılı'), ('Kırıkkale', 'Balışeyh'), ('Kırıkkale', 'Çelebi'), 
    ('Kırıkkale', 'Delice'), ('Kırıkkale', 'Karakeçili'), ('Kırıkkale', 'Keskin'), ('Kırıkkale', 'Sulakyurt'), 
    ('Kırıkkale', 'Yahşihan'),
    
    -- Kırklareli
    ('Kırklareli', 'Kırklareli'), ('Kırklareli', 'Babaeski'), ('Kırklareli', 'Demirköy'), ('Kırklareli', 'Kofçaz'), 
    ('Kırklareli', 'Lüleburgaz'), ('Kırklareli', 'Pehlivanköy'), ('Kırklareli', 'Pınarhisar'), ('Kırklareli', 'Vize'),
    
    -- Kırşehir
    ('Kırşehir', 'Kırşehir'), ('Kırşehir', 'Akçakent'), ('Kırşehir', 'Akpınar'), ('Kırşehir', 'Boztepe'), 
    ('Kırşehir', 'Çiçekdağı'), ('Kırşehir', 'Kaman'), ('Kırşehir', 'Mucur'),
    
    -- Kilis
    ('Kilis', 'Kilis'), ('Kilis', 'Elbeyli'), ('Kilis', 'Musabeyli'), ('Kilis', 'Polateli'),
    
    -- Kocaeli
    ('Kocaeli', 'Başiskele'), ('Kocaeli', 'Çayırova'), ('Kocaeli', 'Darıca'), ('Kocaeli', 'Derince'), 
    ('Kocaeli', 'Dilovası'), ('Kocaeli', 'Gebze'), ('Kocaeli', 'Gölcük'), ('Kocaeli', 'İzmit'), 
    ('Kocaeli', 'Kandıra'), ('Kocaeli', 'Karamürsel'), ('Kocaeli', 'Kartepe'), ('Kocaeli', 'Körfez'),
    
    -- Konya
    ('Konya', 'Ahırlı'), ('Konya', 'Akören'), ('Konya', 'Akşehir'), ('Konya', 'Altınekin'), 
    ('Konya', 'Beyşehir'), ('Konya', 'Bozkır'), ('Konya', 'Cihanbeyli'), ('Konya', 'Çeltik'), 
    ('Konya', 'Çumra'), ('Konya', 'Derbent'), ('Konya', 'Derebucak'), ('Konya', 'Doğanhisar'), 
    ('Konya', 'Emirgazi'), ('Konya', 'Ereğli'), ('Konya', 'Güneysınır'), ('Konya', 'Hadim'), 
    ('Konya', 'Halkapınar'), ('Konya', 'Hüyük'), ('Konya', 'Ilgın'), ('Konya', 'Kadınhanı'), 
    ('Konya', 'Karapınar'), ('Konya', 'Karatay'), ('Konya', 'Kulu'), ('Konya', 'Meram'), 
    ('Konya', 'Sarayönü'), ('Konya', 'Selçuklu'), ('Konya', 'Seydişehir'), ('Konya', 'Taşkent'), 
    ('Konya', 'Tuzlukçu'), ('Konya', 'Yalıhüyük'), ('Konya', 'Yunak'),
    
    -- Kütahya
    ('Kütahya', 'Kütahya'), ('Kütahya', 'Altıntaş'), ('Kütahya', 'Aslanapa'), ('Kütahya', 'Çavdarhisar'), 
    ('Kütahya', 'Domaniç'), ('Kütahya', 'Dumlupınar'), ('Kütahya', 'Emet'), ('Kütahya', 'Gediz'), 
    ('Kütahya', 'Hisarcık'), ('Kütahya', 'Pazarlar'), ('Kütahya', 'Şaphane'), ('Kütahya', 'Simav'), 
    ('Kütahya', 'Tavşanlı'),
    
    -- Malatya
    ('Malatya', 'Akçadağ'), ('Malatya', 'Arapgir'), ('Malatya', 'Arguvan'), ('Malatya', 'Battalgazi'), 
    ('Malatya', 'Darende'), ('Malatya', 'Doğanşehir'), ('Malatya', 'Doğanyol'), ('Malatya', 'Hekimhan'), 
    ('Malatya', 'Kale'), ('Malatya', 'Kuluncak'), ('Malatya', 'Pütürge'), ('Malatya', 'Yazıhan'), 
    ('Malatya', 'Yeşilyurt'),
    
    -- Manisa
    ('Manisa', 'Ahmetli'), ('Manisa', 'Akhisar'), ('Manisa', 'Alaşehir'), ('Manisa', 'Demirci'), 
    ('Manisa', 'Gölmarmara'), ('Manisa', 'Gördes'), ('Manisa', 'Kırkağaç'), ('Manisa', 'Köprübaşı'), 
    ('Manisa', 'Kula'), ('Manisa', 'Salihli'), ('Manisa', 'Sarıgöl'), ('Manisa', 'Saruhanlı'), 
    ('Manisa', 'Selendi'), ('Manisa', 'Soma'), ('Manisa', 'Şehzadeler'), ('Manisa', 'Turgutlu'), 
    ('Manisa', 'Yunusemre'),
    
    -- Mardin
    ('Mardin', 'Artuklu'), ('Mardin', 'Dargeçit'), ('Mardin', 'Derik'), ('Mardin', 'Kızıltepe'), 
    ('Mardin', 'Mazıdağı'), ('Mardin', 'Midyat'), ('Mardin', 'Nusaybin'), ('Mardin', 'Ömerli'), 
    ('Mardin', 'Savur'), ('Mardin', 'Yeşilli'),
    
    -- Mersin
    ('Mersin', 'Akdeniz'), ('Mersin', 'Anamur'), ('Mersin', 'Aydıncık'), ('Mersin', 'Bozyazı'), 
    ('Mersin', 'Çamlıyayla'), ('Mersin', 'Erdemli'), ('Mersin', 'Gülnar'), ('Mersin', 'Mezitli'), 
    ('Mersin', 'Mut'), ('Mersin', 'Silifke'), ('Mersin', 'Tarsus'), ('Mersin', 'Toroslar'), 
    ('Mersin', 'Yenişehir'),
    
    -- Muğla
    ('Muğla', 'Bodrum'), ('Muğla', 'Dalaman'), ('Muğla', 'Datça'), ('Muğla', 'Fethiye'), 
    ('Muğla', 'Kavaklıdere'), ('Muğla', 'Köyceğiz'), ('Muğla', 'Marmaris'), ('Muğla', 'Menteşe'), 
    ('Muğla', 'Milas'), ('Muğla', 'Ortaca'), ('Muğla', 'Seydikemer'), ('Muğla', 'Ula'), 
    ('Muğla', 'Yatağan'),
    
    -- Muş
    ('Muş', 'Muş'), ('Muş', 'Bulanık'), ('Muş', 'Hasköy'), ('Muş', 'Korkut'), 
    ('Muş', 'Malazgirt'), ('Muş', 'Varto'),
    
    -- Nevşehir
    ('Nevşehir', 'Nevşehir'), ('Nevşehir', 'Acıgöl'), ('Nevşehir', 'Avanos'), ('Nevşehir', 'Derinkuyu'), 
    ('Nevşehir', 'Gülşehir'), ('Nevşehir', 'Hacıbektaş'), ('Nevşehir', 'Kozaklı'), ('Nevşehir', 'Ürgüp'),
    
    -- Niğde
    ('Niğde', 'Niğde'), ('Niğde', 'Altunhisar'), ('Niğde', 'Bor'), ('Niğde', 'Çamardı'), 
    ('Niğde', 'Çiftlik'), ('Niğde', 'Ulukışla'),
    
    -- Ordu
    ('Ordu', 'Akkuş'), ('Ordu', 'Altınordu'), ('Ordu', 'Aybastı'), ('Ordu', 'Çamaş'), 
    ('Ordu', 'Çatalpınar'), ('Ordu', 'Çaybaşı'), ('Ordu', 'Fatsa'), ('Ordu', 'Gölköy'), 
    ('Ordu', 'Gülyalı'), ('Ordu', 'Gürgentepe'), ('Ordu', 'İkizce'), ('Ordu', 'Kabadüz'), 
    ('Ordu', 'Kabataş'), ('Ordu', 'Korgan'), ('Ordu', 'Kumru'), ('Ordu', 'Mesudiye'), 
    ('Ordu', 'Perşembe'), ('Ordu', 'Ulubey'), ('Ordu', 'Ünye'),
    
    -- Osmaniye
    ('Osmaniye', 'Osmaniye'), ('Osmaniye', 'Bahçe'), ('Osmaniye', 'Düziçi'), ('Osmaniye', 'Hasanbeyli'), 
    ('Osmaniye', 'Kadirli'), ('Osmaniye', 'Sumbas'), ('Osmaniye', 'Toprakkale'),
    
    -- Rize
    ('Rize', 'Rize'), ('Rize', 'Ardeşen'), ('Rize', 'Çamlıhemşin'), ('Rize', 'Çayeli'), 
    ('Rize', 'Derepazarı'), ('Rize', 'Fındıklı'), ('Rize', 'Güneysu'), ('Rize', 'Hemşin'), 
    ('Rize', 'İkizdere'), ('Rize', 'İyidere'), ('Rize', 'Kalkandere'), ('Rize', 'Pazar'),
    
    -- Sakarya
    ('Sakarya', 'Adapazarı'), ('Sakarya', 'Akyazı'), ('Sakarya', 'Arifiye'), ('Sakarya', 'Erenler'), 
    ('Sakarya', 'Ferizli'), ('Sakarya', 'Geyve'), ('Sakarya', 'Hendek'), ('Sakarya', 'Karapürçek'), 
    ('Sakarya', 'Karasu'), ('Sakarya', 'Kaynarca'), ('Sakarya', 'Kocaali'), ('Sakarya', 'Pamukova'), 
    ('Sakarya', 'Sapanca'), ('Sakarya', 'Serdivan'), ('Sakarya', 'Söğütlü'), ('Sakarya', 'Taraklı'),
    
    -- Samsun
    ('Samsun', 'Alaçam'), ('Samsun', 'Asarcık'), ('Samsun', 'Atakum'), ('Samsun', 'Ayvacık'), 
    ('Samsun', 'Bafra'), ('Samsun', 'Canik'), ('Samsun', 'Çarşamba'), ('Samsun', 'Havza'), 
    ('Samsun', 'İlkadım'), ('Samsun', 'Kavak'), ('Samsun', 'Ladik'), ('Samsun', 'Ondokuzmayıs'), 
    ('Samsun', 'Salıpazarı'), ('Samsun', 'Tekkeköy'), ('Samsun', 'Terme'), ('Samsun', 'Vezirköprü'), 
    ('Samsun', 'Yakakent'),
    
    -- Siirt
    ('Siirt', 'Siirt'), ('Siirt', 'Baykan'), ('Siirt', 'Eruh'), ('Siirt', 'Kurtalan'), 
    ('Siirt', 'Pervari'), ('Siirt', 'Şirvan'), ('Siirt', 'Tillo'),
    
    -- Sinop
    ('Sinop', 'Sinop'), ('Sinop', 'Ayancık'), ('Sinop', 'Boyabat'), ('Sinop', 'Dikmen'), 
    ('Sinop', 'Durağan'), ('Sinop', 'Erfelek'), ('Sinop', 'Gerze'), ('Sinop', 'Saraydüzü'), 
    ('Sinop', 'Türkeli'),
    
    -- Sivas
    ('Sivas', 'Sivas'), ('Sivas', 'Akıncılar'), ('Sivas', 'Altınyayla'), ('Sivas', 'Divriği'), 
    ('Sivas', 'Doğanşar'), ('Sivas', 'Gemerek'), ('Sivas', 'Gölova'), ('Sivas', 'Gürün'), 
    ('Sivas', 'Hafik'), ('Sivas', 'İmranlı'), ('Sivas', 'Kangal'), ('Sivas', 'Koyulhisar'), 
    ('Sivas', 'Suşehri'), ('Sivas', 'Şarkışla'), ('Sivas', 'Ulaş'), ('Sivas', 'Yıldızeli'), 
    ('Sivas', 'Zara'),
    
    -- Şanlıurfa
    ('Şanlıurfa', 'Akçakale'), ('Şanlıurfa', 'Birecik'), ('Şanlıurfa', 'Bozova'), ('Şanlıurfa', 'Ceylanpınar'), 
    ('Şanlıurfa', 'Eyyübiye'), ('Şanlıurfa', 'Halfeti'), ('Şanlıurfa', 'Haliliye'), ('Şanlıurfa', 'Harran'), 
    ('Şanlıurfa', 'Hilvan'), ('Şanlıurfa', 'Karaköprü'), ('Şanlıurfa', 'Siverek'), ('Şanlıurfa', 'Suruç'), 
    ('Şanlıurfa', 'Viranşehir'),
    
    -- Şırnak
    ('Şırnak', 'Şırnak'), ('Şırnak', 'Beytüşşebap'), ('Şırnak', 'Cizre'), ('Şırnak', 'Güçlükonak'), 
    ('Şırnak', 'İdil'), ('Şırnak', 'Silopi'), ('Şırnak', 'Uludere'),
    
    -- Tekirdağ
    ('Tekirdağ', 'Süleymanpaşa'), ('Tekirdağ', 'Çerkezköy'), ('Tekirdağ', 'Çorlu'), ('Tekirdağ', 'Ergene'), 
    ('Tekirdağ', 'Hayrabolu'), ('Tekirdağ', 'Kapaklı'), ('Tekirdağ', 'Malkara'), ('Tekirdağ', 'Marmaraereğlisi'), 
    ('Tekirdağ', 'Muratlı'), ('Tekirdağ', 'Saray'), ('Tekirdağ', 'Şarköy'),
    
    -- Tokat
    ('Tokat', 'Tokat'), ('Tokat', 'Almus'), ('Tokat', 'Artova'), ('Tokat', 'Başçiftlik'), 
    ('Tokat', 'Erbaa'), ('Tokat', 'Niksar'), ('Tokat', 'Pazar'), ('Tokat', 'Reşadiye'), 
    ('Tokat', 'Sulusaray'), ('Tokat', 'Turhal'), ('Tokat', 'Yeşilyurt'), ('Tokat', 'Zile'),
    
    -- Trabzon
    ('Trabzon', 'Ortahisar'), ('Trabzon', 'Akçaabat'), ('Trabzon', 'Araklı'), ('Trabzon', 'Arsin'), 
    ('Trabzon', 'Beşikdüzü'), ('Trabzon', 'Çarşıbaşı'), ('Trabzon', 'Çaykara'), ('Trabzon', 'Dernekpazarı'), 
    ('Trabzon', 'Düzköy'), ('Trabzon', 'Hayrat'), ('Trabzon', 'Köprübaşı'), ('Trabzon', 'Maçka'), 
    ('Trabzon', 'Of'), ('Trabzon', 'Sürmene'), ('Trabzon', 'Şalpazarı'), ('Trabzon', 'Tonya'), 
    ('Trabzon', 'Vakfıkebir'), ('Trabzon', 'Yomra'),
    
    -- Tunceli
    ('Tunceli', 'Tunceli'), ('Tunceli', 'Çemişgezek'), ('Tunceli', 'Hozat'), ('Tunceli', 'Mazgirt'), 
    ('Tunceli', 'Nazımiye'), ('Tunceli', 'Ovacık'), ('Tunceli', 'Pertek'), ('Tunceli', 'Pülümür'),
    
    -- Uşak
    ('Uşak', 'Uşak'), ('Uşak', 'Banaz'), ('Uşak', 'Eşme'), ('Uşak', 'Karahallı'), 
    ('Uşak', 'Sivaslı'), ('Uşak', 'Ulubey'),
    
    -- Van
    ('Van', 'İpekyolu'), ('Van', 'Bahçesaray'), ('Van', 'Başkale'), ('Van', 'Çaldıran'), 
    ('Van', 'Çatak'), ('Van', 'Edremit'), ('Van', 'Erciş'), ('Van', 'Gevaş'), 
    ('Van', 'Gürpınar'), ('Van', 'Muradiye'), ('Van', 'Özalp'), ('Van', 'Saray'), 
    ('Van', 'Tuşba'),
    
    -- Yalova
    ('Yalova', 'Yalova'), ('Yalova', 'Altınova'), ('Yalova', 'Armutlu'), ('Yalova', 'Çınarcık'), 
    ('Yalova', 'Çiftlikköy'), ('Yalova', 'Termal'),
    
    -- Yozgat
    ('Yozgat', 'Yozgat'), ('Yozgat', 'Akdağmadeni'), ('Yozgat', 'Aydıncık'), ('Yozgat', 'Boğazlıyan'), 
    ('Yozgat', 'Çandır'), ('Yozgat', 'Çayıralan'), ('Yozgat', 'Çekerek'), ('Yozgat', 'Kadışehri'), 
    ('Yozgat', 'Saraykent'), ('Yozgat', 'Sarıkaya'), ('Yozgat', 'Sorgun'), ('Yozgat', 'Şefaatli'), 
    ('Yozgat', 'Yenifakılı'), ('Yozgat', 'Yerköy'),
    
    -- Zonguldak
    ('Zonguldak', 'Zonguldak'), ('Zonguldak', 'Alaplı'), ('Zonguldak', 'Çaycuma'), ('Zonguldak', 'Devrek'), 
    ('Zonguldak', 'Ereğli'), ('Zonguldak', 'Gökçebey'), ('Zonguldak', 'Kilimli'), ('Zonguldak', 'Kozlu')
) AS d("CityName", "Name")
INNER JOIN "Cities" c ON c."Name" = d."CityName"
ON CONFLICT DO NOTHING;

