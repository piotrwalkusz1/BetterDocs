import React, { useEffect, useState } from "react";
import { Admin, DataProvider, Resource } from "react-admin";
import buildGraphQLProvider from "./data-provider/graphqlDataProvider";
import { theme } from "./theme/theme";
import Login from "./Login";
import "./App.scss";
import Dashboard from "./pages/Dashboard";
import { UserList } from "./user/UserList";
import { UserCreate } from "./user/UserCreate";
import { UserEdit } from "./user/UserEdit";
import { UserShow } from "./user/UserShow";
import { AnimeList } from "./anime/AnimeList";
import { AnimeCreate } from "./anime/AnimeCreate";
import { AnimeEdit } from "./anime/AnimeEdit";
import { AnimeShow } from "./anime/AnimeShow";
import { SeasonList } from "./season/SeasonList";
import { SeasonCreate } from "./season/SeasonCreate";
import { SeasonEdit } from "./season/SeasonEdit";
import { SeasonShow } from "./season/SeasonShow";
import { jwtAuthProvider } from "./auth-provider/ra-auth-jwt";

const App = (): React.ReactElement => {
  const [dataProvider, setDataProvider] = useState<DataProvider | null>(null);
  useEffect(() => {
    buildGraphQLProvider
      .then((provider: any) => {
        setDataProvider(() => provider);
      })
      .catch((error: any) => {
        console.log(error);
      });
  }, []);
  if (!dataProvider) {
    return <div>Loading</div>;
  }
  return (
    <div className="App">
      <Admin
        title={"Anime Review"}
        dataProvider={dataProvider}
        authProvider={jwtAuthProvider}
        theme={theme}
        dashboard={Dashboard}
        loginPage={Login}
      >
        <Resource
          name="User"
          list={UserList}
          edit={UserEdit}
          create={UserCreate}
          show={UserShow}
        />
        <Resource
          name="Anime"
          list={AnimeList}
          edit={AnimeEdit}
          create={AnimeCreate}
          show={AnimeShow}
        />
        <Resource
          name="Season"
          list={SeasonList}
          edit={SeasonEdit}
          create={SeasonCreate}
          show={SeasonShow}
        />
      </Admin>
    </div>
  );
};

export default App;
