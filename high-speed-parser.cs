#include <iostream>
#include <cassert>
#include <sstream>
#include <string>
#include <regex>
#include <map>


void descriptionRegexError(const std::regex_error& e);


template<class callback>
void search(const std::string& from, const std::string& pattern, const callback& signal)
{
    const std::regex reg(pattern);
    std::sregex_token_iterator i(from.begin(), from.end(), reg);
    std::sregex_token_iterator end;
    while (i != end)
        if(signal(i->str()))
             break;
        else
           ++i;
}


int main()
{
    setlocale(LC_ALL, "");
    std::cout <<"пример регулярного выражения\n";


    const char* source =
        "ssl1001 129960997 2014-01-21T08:36:33.097 0.426 1.2.3.4 -/200 12324 "
        "GET [url]https://en.wikipedia.org/w/index.php...edit&section=8[/url] "
        " NONE/wikimedia - [url]https://en.wikipedia.org/wiki/Kirschkuchen[/url] - Mozilla/5.0 (Windows NT 5.1) "
        "AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1700.76 Safari/537.36 en-US,en;q=0.8 -"
        "sq18.wikimedia.org 1715898 2010-12-01T21:57:22.331 0 1.2.3.4 TCP_MEM_HIT/200 13208 "
        "GET [url]http://en.wikipedia.org/wiki/Main_Page[/url]  NONE/- text/html - - "
        "Mozilla/4.0%20(compatible;%20MSIE%206.0;%20Windows%20NT%205.1;%20.NET%20CLR%201.1.4322) en-US -"
        "cp1048.eqiad.wmnet 8883921154 2013-09-26T06:28:16 0.001308203 1.2.3.4 hit/200 52362 "
        "GET [url]http://en.wikipedia.org/wiki/Free_software[/url]  - text/html [url]https://www.google.com/search?q=free+software[/url]    "
        "4.3.2.1 Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) "
        "Chrome/29.0.1547.76 Safari/537.36 en-US,en;q=0.8 - ";


    struct descript
    {
        size_t count = {};
        std::map<std::string, size_t>
            statistic = {};
    };


    std::map<std::string, descript>
        data;


    size_t tolalurls = 0;
    const auto process_url_path = [&data, &tolalurls](const std::string& found)
    {
        ++tolalurls;
        std::string url = found;
        std::cout << "найдено: " << url << std::endl;
        assert(std::cout);


        std::string domain;
        const auto get_domain = [&domain](const std::string& found)
            { return domain = found.substr(2), true; };
        const std::string domain_regex = "//[a-zA-Z0-9.-]*";
        search(url, domain_regex, get_domain);
        std::cout << "  домен: " << domain << std::endl;


        const auto beg = url.find(domain);
        assert(beg!=url.npos);


        url = url.substr(beg + domain.length());


        std::string path;
        const auto get_path = [&path](const std::string& found)
            { return path = found, true; };
        const std::string path_regex = "[a-zA-Z0-9,/+_]*\\.?[a-zA-Z0-9,/+_]*";
        search(url, path_regex, get_path);
        if(path.empty())
            path = '/';
        std::cout << "  путь: " << path << std::endl;


        auto& record = data[domain];
        ++record.count;
        ++record.statistic[path];
        return false;
    };


    const std::string fast_url_regex
        = "(https?)://[^\\s]*";


    try
    {
        search(source, fast_url_regex, process_url_path);


        size_t totalpaths = 0;


        for(const auto& sub: data)
            for(const auto& pair: sub.second.statistic)
                (void)pair,
                ++totalpaths;


        std::cout
            << "\n\nвсего url "
            << tolalurls
            << ", доменов "
            << data.size()
            <<", путей "
            << totalpaths
            << std::endl;


        std::cout << "\nтоп доменов:\n";
        for(const auto& pair: data)
        {
            const auto& domain = pair.first;
            const auto& record = pair.second;
            std::cout << "  " << record.count << " "
                << domain << '\n';
        }


        std::cout << "\nтоп путей:\n";
        for(const auto& pair: data)
        {
            const auto& record = pair.second;
            for(const auto& subpair: record.statistic)
            {
                const auto& path  = subpair.first;
                const auto& count = subpair.second;
                std::cout << "  " << count << " "
                    << path << '\n';
            }
        }
    }
    catch(const std::regex_error& e)
        { descriptionRegexError(e); }


    std::cout << "закончено\n";
}




void descriptionRegexError(const std::regex_error& e)
{
    std::cout << "Ошибка std::regex: " << e.what() << std::endl;
   
    if(e.code()==std::regex_constants::error_collate)
        std::cout << "выражение содержит недопустимый элемент сравнения\n";  
    else if(e.code()==std::regex_constants::error_ctype)
        std::cout << "выражение содержит недопустимое имя класса символов\n";
    else if(e.code()==std::regex_constants::error_escape)
        std::cout << "выражение содержит недопустимый экранированный символ или завершающий экранированный символ\n";
    else if(e.code()==std::regex_constants::error_backref)
        std::cout << "выражение содержит недопустимую обратную ссылку\n";
    else if(e.code()==std::regex_constants::error_brack)
        std::cout << "выражение содержит несоответствие квадратных скобок ('[' и ']')\n";
    else if(e.code()==std::regex_constants::error_paren)
        std::cout << "выражение содержит несоответствие круглых скобок ('(' и ')')\n";
    else if(e.code()==std::regex_constants::error_brace)
        std::cout << "выражение содержит несоответствие фигурных скобок ('{' и '}')\n";
    else if(e.code()==std::regex_constants::error_badbrace)
        std::cout << "выражение содержит недопустимый диапазон в выражении {}\n";
    else if(e.code()==std::regex_constants::error_space)
        std::cout << "не хватило памяти для преобразования выражения в конечный автомат\n";
    else if(e.code()==std::regex_constants::error_badrepeat)
        std::cout << "один из *?+{ не предшествует действительному регулярному выражению\n";
    else if(e.code()==std::regex_constants::error_complexity)
        std::cout << "сложность попытки сопоставления превысила предопределенный уровень\n";
    else if(e.code()==std::regex_constants::error_stack)
        std::cout << "не хватило памяти для выполнения сопоставления\n";
 
    #ifdef _MSC_VER  
    else if(e.code()==std::regex_constants::error_parse)
        std::cout << "ошибка разбора\n";
    else if(e.code()==std::regex_constants::error_syntax)
        std::cout << "синтаксическая ошибка\n";
    #endif
 
    else
        std::cout << "неизвестная ошибка std::regex\n";
} 
