Добавляем общие стили в файл ```custom.scss```

```css
body {
    padding-top: 60px;
}

section {
    overflow: auto;
}

textarea {
    resize: vertical;
}

.center {
    text-align: center;
}

.center h1 {
    margin-bottom: 10px;
}

```

Добавим в ```custom.scss``` дополнительные правила CSS для улучшения оформления
текста.

```css
h1, h2, h3, h4, h5, h6 {
    line-height: 1;
}

h1 {
    font-size: 3em;
    letter-spacing: -2px;
    margin-bottom: 30px;
    text-align: center;
}

h2 {
    font-size: 1.2em;
    letter-spacing: -1px;
    margin-bottom: 30px;
    text-align: center;
    font-weight: normal;
    color: #777;
}

p {
    font-size: 1.1em;
    line-height: 1.7em;
}
```

Делаем в ```custom.scss``` правила для оформления логотипа (в результате лого меняется). 

```css
#logo {
    float: left;
    margin-right: 10px;
    font-size: 1.7em;
    color: #fff;
    text-transform: uppercase;
    letter-spacing: -1px;
    padding-top: 9px;
    font-weight: bold;
}

#logo:hover {
    color: #fff;
    text-decoration: none;
}
```