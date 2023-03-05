package com.example.tomanota;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.Gravity;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.PopupWindow;
import android.widget.TableLayout;
import android.widget.TableRow;
import android.widget.TextView;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.Volley;
import com.google.gson.Gson;
import com.google.gson.JsonArray;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

public class ReviewActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_review);

        Bundle b = getIntent().getExtras();
        String title = "";
        if (b != null)
        {
            title = b.getString("Title");
        }

        final Button btnBack = (Button) findViewById(R.id.buttonBack);

        btnBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                finish();
            }
        });

        final TableLayout promptList = (TableLayout) findViewById(R.id.promptTable);

        String URL = "https://europe-west1-tomanota-374115.cloudfunctions.net/get-prompts?q=" + title;
        List<List<String>> prompts = new ArrayList<>();
        Gson gson = new Gson();

        RequestQueue requestQueue = Volley.newRequestQueue(ReviewActivity.this);

        JsonArrayRequest objectRequest = new JsonArrayRequest(
                Request.Method.GET,
                URL,
                null,
                response -> {
                    for (int i = 0; i < response.length(); i++) {
                        try {
                            JSONArray promptInner = response.getJSONArray(i);
                            List<String> promptSet = new ArrayList<>();
                            promptSet.add(promptInner.getString(0));
                            promptSet.add(promptInner.getString(1));
                            prompts.add(promptSet);
                        }
                        catch (JSONException e)
                        {
                            e.printStackTrace();
                        }
                    }
                    Log.d("Expected", "List of prompts: " + prompts.get(0).toString());

                    promptList.removeAllViews();
                    for (int i = 0; i < prompts.size(); i++)
                    {
                        List<String> innerPrompts = prompts.get(i);
                        TableRow tableRow = new TableRow(this);

                        for (int j = 0; j < innerPrompts.size(); j++)
                        {
                            String prompt = innerPrompts.get(j);
                            TextView textView = new TextView(this);
                            textView.setText(prompt);
                            tableRow.addView(textView);
                        }
                        promptList.addView(tableRow);
                    }

                },
                error -> {
                    Log.e("TAG", "Error: " + error.getMessage());
                }
        );

        requestQueue.add(objectRequest);
    }
}