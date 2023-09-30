import * as React from "react";

import {
  Edit,
  SimpleForm,
  EditProps,
  NumberInput,
  TextInput,
  ReferenceArrayInput,
  SelectArrayInput,
} from "react-admin";

import { SeasonTitle } from "../season/SeasonTitle";

export const AnimeEdit = (props: EditProps): React.ReactElement => {
  return (
    <Edit {...props}>
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
    </Edit>
  );
};
