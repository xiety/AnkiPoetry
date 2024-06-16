# AnkiPoetry

Generates an Anki deck from poetry.

Use online: https://xiety.github.io/AnkiPoetry/

Example deck: https://ankiweb.net/shared/info/574333410

Direct link: [docs/William_Shakespeare_-_Hamlet_monologue.apkg](https://github.com/xiety/AnkiPoetry/raw/main/docs/William_Shakespeare_-_Hamlet_monologue.apkg)

## Warning

When importing `.csv` file into Anki make sure that "Mach scope" is set to "Notetype and deck" to prevent cards in other decks from being updated.

## UI

![Screenshot](docs/Screenshot_01.png?raw=true)

## Fundamentals

- My main goal is not speed of memorization, but enjoyment of the process
- All text is divided into pages with the specified number of lines. I try to choose it so that the page is fully visible on the phone screen.
- The last line of every page is repeated on the top of the next page
- You always see all the previous lines on the current page. Think of it as a little cheating. It is not necessary to read them all every time, they just need to be visible.
- The first line of the next chapter appears as the last line of the current chapter. And vice versa.
- Lines are colorized starting from the first line of the chapter. Colors help me remember lines better. But you can change or remove them in card styles. And you can use more than 6, but you will need to add new styles to the card.
- Word Wrap splits lines based on punctuation marks not on spaces
- If I use hint, then it's almost certainly the Again button

## Stages of memorization

1. Word

At first I only learn words. This is quite easy to do and allows me to become intimately familiar with the text. Usually this is 10-20 new words a day, so as not to stay long.

![Screenshot](docs/word.gif?raw=true)

2. Line

After a while, when I feel more confident, I move on to this deck to study the lines. Usually this is 1-3 new lines per day.

![Screenshot](docs/line.gif?raw=true)

3. Page

Soon after the lines deck have moved to the next page, I reveal two new cards on this deck, to learn odd and even lines of the completed page.

![Screenshot](docs/page.gif?raw=true)

## Input format

- `#` - the title of the book
- `##` - the title of the chapter

## Parameters

- `Deck Name` - parent deck name to add to .csv file to simplify import
- `Chunk size` - number of lines per page
- `Word Wrap` - number of characters after which to try to split the line
- `On spaces` - wrap text on spaces or on punctuation
- `Add dots` - add dots before and after text on wrap
- `Colors` - number of colors to alternate
- `Overlap` - add first line to the end of previous chapter and vice versa
- `Numbers` - add line number to every line
- `Continous` - continous line numbers through all text

## Importing created deck into Anki

First import a generic deck that will contain all the required notetypes:

AnkiWeb: https://ankiweb.net/shared/info/574333410

Direct link: [docs/William_Shakespeare_-_Hamlet_monologue.apkg](https://github.com/xiety/AnkiPoetry/raw/main/docs/William_Shakespeare_-_Hamlet_monologue.apkg)

Then create a parent deck and three subdecks: `1. word`, `2. line`, `3. page`. And import each `.csv` into the deck created for it.

In the end it will look something like this:

![Screenshot](docs/Screenshot_03.png?raw=true)

## TODO

I'm thinking about generating an `.apkg` file right away. Or maybe using `CrowdAnki` `json` format.
