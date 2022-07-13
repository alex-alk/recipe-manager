using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace recipe_manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private SQLiteConnection connection = new SQLiteConnection(@"Data Source=../../../recipe.sqlite");
        public MainWindow()
        {
            InitializeComponent();

            ShowRecipes();
            ShowAllIngredients();
        }

        private void ShowAllIngredients()
        {

            try
            {
                var cmd = new SQLiteCommand();
                cmd = connection.CreateCommand();
                cmd.CommandText = "select * from Ingredient";

                SQLiteDataAdapter sqlDataAdapter = new SQLiteDataAdapter(cmd);
                using (sqlDataAdapter)
                {
                    DataTable ingredientTable = new DataTable();
                    sqlDataAdapter.Fill(ingredientTable);

                    listAllIngredients.DisplayMemberPath = "Name";
                    listAllIngredients.SelectedValuePath = "Id";
                    listAllIngredients.ItemsSource = ingredientTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void ShowRecipes()
        {
            try
            {
                var cmd = new SQLiteCommand();
                cmd = connection.CreateCommand();
                cmd.CommandText = "select * from recipe";

                SQLiteDataAdapter sqlDataAdapter = new SQLiteDataAdapter(cmd);
                using (sqlDataAdapter)
                {
                    DataTable recipeTable = new DataTable();
                    sqlDataAdapter.Fill(recipeTable);
                    listRecipes.DisplayMemberPath = "Name";
                    listRecipes.SelectedValuePath = "Id";
                    listRecipes.ItemsSource = recipeTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ShowAssociatedIngredients()
        {


            try
            {
                string query = "select * from Ingredient a inner join RecipeIngredient " +
                        "za on a.Id = za.IngredientId where za.RecipeId = @RecipeId";

                var sqlCommand = new SQLiteCommand();
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;

                SQLiteDataAdapter sqlDataAdapter = new SQLiteDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@RecipeId", listRecipes.SelectedValue);
                    DataTable ingredientTable = new DataTable();
                    sqlDataAdapter.Fill(ingredientTable);
                    listAssociatedIngredients.DisplayMemberPath = "Name";
                    listAssociatedIngredients.SelectedValuePath = "Id";
                    listAssociatedIngredients.ItemsSource = ingredientTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void listRecipes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAssociatedIngredients();
            ShowSelectedRecipeInTextBox();
        }

        private void DeleteRecipe_Click(object sender, RoutedEventArgs e)
        {
            //Recipe recipe = dataContext.Recipes.FirstOrDefault(z => z.Id == Convert.ToInt32(listRecipes.SelectedValue));
            //dataContext.Recipes.DeleteOnSubmit(recipe);
            //dataContext.SubmitChanges();
            //ShowRecipes();

            try
            {
                connection.Open();
                string query = "delete from Recipe where id = @RecipeId";

                var sqlCommand = new SQLiteCommand();
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;

                sqlCommand.Parameters.AddWithValue("@RecipeId", listRecipes.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            finally
            {
                connection.Close();
                ShowRecipes();
            }

        }

        private void AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            //Recipe recipe = new Recipe();
            //recipe.Name = myTextBox.Text;
            //dataContext.Recipes.InsertOnSubmit(recipe);
            //dataContext.SubmitChanges();
            //ShowRecipes();

            try
            {
                string query = "insert into Recipe (Name) values (@Name)";
                connection.Open();

                var sqlCommand = new SQLiteCommand();
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;

                sqlCommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            finally
            {
                connection.Close();
                ShowRecipes();
            }
        }

        private void ShowSelectedIngredientInTextBox()
        {
            //var ingredient = dataContext.Ingredients.FirstOrDefault(a => a.Id == Convert.ToInt32(listAllIngredients.SelectedValue));
            //if (ingredient != null)
            //{
            //myTextBox.Text = ingredient.Name;
            //}

            try
            {
                string query = "select name from Ingredient where Id = @IngredientId";
                var sqlCommand = new SQLiteCommand();
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;

                SQLiteDataAdapter sqlDataAdapter = new SQLiteDataAdapter(sqlCommand);
                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@IngredientId", listAllIngredients.SelectedValue);
                    DataTable recipeDataTable = new DataTable();
                    sqlDataAdapter.Fill(recipeDataTable);
                    if (recipeDataTable.Rows.Count > 0)
                    {
                        myTextBox.Text = recipeDataTable.Rows[0]["Name"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ShowSelectedRecipeInTextBox()
        {
            //var recipe = dataContext.Recipes.FirstOrDefault(z => z.Id == Convert.ToInt32(listRecipes.SelectedValue));
            //if(recipe != null)
            //{
            //    myTextBox.Text = recipe.Name;
            //}

            try
            {
                string query = "select name from Recipe where Id = @RecipeId";
                var sqlCommand = new SQLiteCommand();
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;

                SQLiteDataAdapter sqlDataAdapter = new SQLiteDataAdapter(sqlCommand);
                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@RecipeId", listRecipes.SelectedValue);
                    DataTable recipeDataTable = new DataTable();
                    sqlDataAdapter.Fill(recipeDataTable);
                    if (recipeDataTable.Rows.Count > 0)
                    {
                        myTextBox.Text = recipeDataTable.Rows[0]["Name"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            //Ingredient ingredient = new Ingredient();
            //ingredient.Name = myTextBox.Text;
            //dataContext.Ingredients.InsertOnSubmit(ingredient);
            //dataContext.SubmitChanges();

            try
            {
                string query = "insert into Ingredient (Name) values (@Name)";
                connection.Open();

                var sqlCommand = new SQLiteCommand();
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;

                sqlCommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            finally
            {
                connection.Close();
                ShowAllIngredients();
            }
        }

        private void addIngredientToRecipe_Click(object sender, RoutedEventArgs e)
        {

            //RecipeIngredient za = new RecipeIngredient();
            //za.RecipeId = Convert.ToInt32(listRecipes.SelectedValue);
            //za.IngredientId = Convert.ToInt32(listAllIngredients.SelectedValue);
            //dataContext.RecipeIngredients.InsertOnSubmit(za);
            //dataContext.SubmitChanges();
            //ShowAssociatedIngredients();

            try
            {
                string query = "insert into RecipeIngredient (RecipeId, IngredientId) values (@RecipeId, @IngredientId)";

                connection.Open();

                var sqlCommand = new SQLiteCommand();
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;

                sqlCommand.Parameters.AddWithValue("@RecipeId", listRecipes.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@IngredientId", listAllIngredients.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                connection.Close();
                ShowAssociatedIngredients();
            }

        }

        private void UpdateRecipe_Click(object sender, RoutedEventArgs e)
        {

            //Recipe recipe = dataContext.Recipes.FirstOrDefault(z => z.Id == Convert.ToInt32(listRecipes.SelectedValue));
            //if (recipe != null)
            //{
            //recipe.Name = myTextBox.Text;
            //dataContext.SubmitChanges();
            //ShowRecipes();
            //}

            try
            {
                string query = "update Recipe Set Name = @Name where Id = @RecipeId";

                connection.Open();

                var sqlCommand = new SQLiteCommand();
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;

                sqlCommand.Parameters.AddWithValue("@RecipeId", listRecipes.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            finally
            {
                connection.Close();
                ShowRecipes();
            }

        }

        private void UpdateIngredient_Click(object sender, RoutedEventArgs e)
        {
            /*
            Ingredient ingredient = dataContext.Ingredients.FirstOrDefault(a => a.Id == Convert.ToInt32(listAllIngredients.SelectedValue));
            if (ingredient != null)
            {
                ingredient.Name = myTextBox.Text;
                dataContext.SubmitChanges();
                ShowAllIngredients();
            }*/

            try
            {
                string query = "update Ingredient Set Name = @Name where Id = @IngredientId";
                connection.Open();

                var sqlCommand = new SQLiteCommand();
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;

                sqlCommand.Parameters.AddWithValue("@IngredientId", listAllIngredients.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                connection.Close();
                ShowAllIngredients();
            }

        }

        private void DeleteIngredient_Click(object sender, RoutedEventArgs e)
        {
            /*Ingredient ingredient = dataContext.Ingredients.FirstOrDefault(a => a.Id == Convert.ToInt32(listAllIngredients.SelectedValue));
            dataContext.Ingredients.DeleteOnSubmit(ingredient);
            dataContext.SubmitChanges();
            ShowAllIngredients();*/

            try
            {
                string query = "delete from Ingredient where id = @IngredientId";
                connection.Open();

                var sqlCommand = new SQLiteCommand();
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;
                sqlCommand.Parameters.AddWithValue("@IngredientId", listAllIngredients.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                connection.Close();
                ShowAllIngredients();
            }
        }



        private void listAllIngredients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedIngredientInTextBox();
        }

        private void RemoveAssociatedIngredient_Click(object sender, RoutedEventArgs e)
        {
            //RecipeIngredient za = dataContext.RecipeIngredients.FirstOrDefault(a => a.Id == Convert.ToInt32(listAssociatedIngredients.SelectedValue));
            //dataContext.RecipeIngredients.DeleteOnSubmit(za);
            //dataContext.SubmitChanges();
            //ShowAssociatedIngredients();

            try
            {
                string query = "delete from RecipeIngredient where IngredientId = @IngredientId";
                connection.Open();

                var sqlCommand = new SQLiteCommand();
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;
                sqlCommand.Parameters.AddWithValue("@IngredientId", listAssociatedIngredients.SelectedValue);

                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                connection.Close();
                ShowAssociatedIngredients();
            }

        }
    }
}
