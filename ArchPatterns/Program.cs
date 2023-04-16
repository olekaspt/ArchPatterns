// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// This is a variantion of the MVC in that the controller is the part that takes the 
// the inputs\actions and is responsible for notifiying the view

Controller.PartController m_partController = new Controller.PartController();
View.PartView m_partView = new View.PartView();

m_partController.Subscribe(m_partView);

m_partController.CreatePart("Part1", "Drafting");

m_partController.CreatePart("Part2", "Drafting");
m_partController.CreatePart("Part3", "Drafting");
m_partController.CreatePart("Part4", "Drafting");

m_partController.RemovePart("Part2");

m_partController.CloseAllParts();

