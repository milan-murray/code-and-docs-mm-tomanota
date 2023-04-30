package com.example.tomanota;

import android.content.Intent;
import android.os.Bundle;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.Gravity;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.PopupWindow;
import android.widget.ScrollView;
import android.widget.TableLayout;
import android.widget.TextView;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.Volley;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

public class SelectionActivity extends AppCompatActivity {

    private String selectedTitle = "";

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Intent intent = getIntent();

        String user = "";
        if (intent.getStringExtra("email") != null)
        {
            user = intent.getStringExtra("email");
        }

        final TableLayout userTitlesList = findViewById(R.id.user_table_titles);

        final TextView textViewSelected = findViewById(R.id.selectedTextID);
        textViewSelected.setText(selectedTitle);

        final Button btnReview = findViewById(R.id.btnReview);
        btnReview.setEnabled(false);

        String key = "X00162027";
        String baseURI = "https://user-web-api.azurewebsites.net/db";
        String URL = baseURI + "/key/" + key + "/user/" + user;
        List<String> titles = new ArrayList<>();
        // Gson gson = new Gson();

        RequestQueue requestQueue = Volley.newRequestQueue(SelectionActivity.this);

        JsonArrayRequest objectRequest = new JsonArrayRequest(
                Request.Method.GET,
                URL,
                null,
                response -> {
                    for (int i = 0; i < response.length(); i++) {
                        try {
                            JSONObject title = response.getJSONObject(i);
                            titles.add(title.getString("title"));
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
                    }

                    Log.d("Expected", "List of titles: " + titles);

                    for (int i = 0; i < titles.size(); i++) {
                        Button button = new Button(SelectionActivity.this);
                        button.setText(titles.get(i));

                        button.setLayoutParams(new LinearLayout.LayoutParams(
                                LinearLayout.LayoutParams.MATCH_PARENT,
                                LinearLayout.LayoutParams.WRAP_CONTENT));
                        userTitlesList.addView(button);

                        button.setOnClickListener(new View.OnClickListener() {
                            @Override
                            public void onClick(View view) {
                                selectedTitle = button.getText().toString();
                                textViewSelected.setText(selectedTitle);
                                btnReview.setEnabled(true);
                            }
                        });
                    }
                },
                error -> {
                    Log.e("TAG", "Error: " + error.getMessage());
                }
        );

        requestQueue.add(objectRequest);

        btnReview.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(SelectionActivity.this, ReviewActivity.class);
                Bundle b = new Bundle();
                b.putString("Title", selectedTitle);
                // b.putBoolean("Premade", );
                intent.putExtras(b);
                startActivity(intent);
            }
        });

        final Button btnTest = findViewById(R.id.btnTest);
        btnTest.setEnabled(false);

        btnTest.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                startActivity(new Intent(SelectionActivity.this, TestActivity.class));
            }
        });

        final Button btnSpanish = findViewById(R.id.btnSpanish);
        btnSpanish.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                DisplayMetrics displayMetrics = new DisplayMetrics();
                getWindowManager().getDefaultDisplay().getMetrics(displayMetrics);
                int screenWidth = displayMetrics.widthPixels;
                int screenHeight = displayMetrics.heightPixels;

                int width = (int) (screenWidth * 0.8f);
                int height = (int) (screenHeight * 0.8f);

                // Get titles
                View popupView = getLayoutInflater().inflate(R.layout.popup_titles, null);
                ScrollView popupScrollView = popupView.findViewById(R.id.popupScrollView);
                LinearLayout popupLayout = popupScrollView.findViewById(R.id.linearLayoutTitles);

                String URL = "https://europe-west1-tomanota-374115.cloudfunctions.net/get-EN-titles-ES";
                List<String> titles = new ArrayList<>();
                // Gson gson = new Gson();

                RequestQueue requestQueue = Volley.newRequestQueue(SelectionActivity.this);

                JsonArrayRequest objectRequest = new JsonArrayRequest(
                        Request.Method.GET,
                        URL,
                        null,
                        response -> {
                            for (int i = 0; i < response.length(); i++) {
                                try {
                                    String title = response.getString(i);
                                    titles.add(title);
                                } catch (JSONException e) {
                                    e.printStackTrace();
                                }
                            }

                            Log.d("Expected", "List of titles: " + titles);

                            boolean focusable = true;
                            final PopupWindow popupWindow = new PopupWindow(popupView, width, height, focusable);

                            popupWindow.showAtLocation(view, Gravity.CENTER, 0, 0);
                            for (int i = 0; i < titles.size(); i++) {
                                Button button = new Button(SelectionActivity.this);
                                button.setText(titles.get(i));

                                button.setLayoutParams(new LinearLayout.LayoutParams(
                                        LinearLayout.LayoutParams.MATCH_PARENT,
                                        LinearLayout.LayoutParams.WRAP_CONTENT));
                                popupLayout.addView(button);

                                button.setOnClickListener(new View.OnClickListener() {
                                    @Override
                                    public void onClick(View view) {
                                        selectedTitle = button.getText().toString();
                                        popupWindow.dismiss();
                                        textViewSelected.setText(selectedTitle);
                                        btnReview.setEnabled(true);
                                    }
                                });
                            }
                        },
                        error -> {
                            Log.e("TAG", "Error: " + error.getMessage());
                        }
                );

                requestQueue.add(objectRequest);


            }
        });
    }
}
