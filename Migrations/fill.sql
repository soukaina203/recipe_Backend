INSERT INTO
    "Categories" ("Nom", "Image")
VALUES (
        'Desserts',
        'https://example.com/images/desserts.jpg'
    );

INSERT INTO
    "Recipes" (
        "Title",
        "Description",
        "Ingredients",
        "Instructions",
        "Image",
        "IdUser",
        "IdCategory"
    )
VALUES (
        'Chocolate Cake',
        'A rich and moist chocolate cake recipe.',
        'Flour, sugar, cocoa powder, eggs, butter, baking powder, milk',
        '1. Mix ingredients\n2. Bake at 180°C for 35 minutes\n3. Let it cool',
        'https://example.com/images/chocolate-cake.jpg',
        1,
        1
    );
SELECT "Id" FROM "Categories" WHERE "Nom" = 'Desserts';

UPDATE "Users" SET "IsAdmin" = 1 WHERE "Id" = 2;
