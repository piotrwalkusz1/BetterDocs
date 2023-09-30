import * as React from "react";

import {
  Create,
  SimpleForm,
  CreateProps,
  NumberInput,
  TextInput,
  ReferenceArrayInput,
  SelectArrayInput,
} from "react-admin";

import { SeasonTitle } from "../season/SeasonTitle";

export const AnimeCreate = (props: CreateProps): React.ReactElement => {
  return (
    <Create {...props}>
      <SimpleForm>
        <NumberInput step={1} label="Episodes count" source="episodesCount" />
        <TextInput label="Name" source="name" />
        <ReferenceArrayInput
          source="seasons"
          reference="Season"
          parse={(value: any) => value && value.map((v: any) => ({ id: v }))}
          format={(value: any) => value && value.map((v: any) => v.id)}
        >
          <SelectArrayInput optionText={SeasonTitle} />
        </ReferenceArrayInput>
      </SimpleForm>
    </Create>
  );
};
